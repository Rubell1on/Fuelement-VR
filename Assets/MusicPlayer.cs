using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class MusicPlayer : MonoBehaviour
{
    public List<MusicTrack> tracks;
    [Space(10)]
    public MusicTrack currentTrack;
    public AudioSource audioSource;

    [Header("Controls")]
    public KeyCode prevTrackButton = KeyCode.LeftBracket;
    public KeyCode nextTrackButton = KeyCode.RightBracket;

    [Header("Events")]
    public MusicPlayerEvent onTrackChanged;

    Coroutine timer;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;

        currentTrack = tracks[0];
        Play(currentTrack);
    }

    void Update()
    {
        if (Input.GetKeyUp(prevTrackButton)) PreviousTrack();
        if (Input.GetKeyUp(nextTrackButton)) NextTrack();
    }

    public void NextTrack()
    {
        int trackId = GetTrackId(currentTrack);

        currentTrack = trackId + 1 > tracks.Count - 1 ? tracks[0] : tracks[trackId + 1];
        Play(currentTrack);
    }

    public void PreviousTrack()
    {
        int trackId = GetTrackId(currentTrack);

        currentTrack = trackId - 1 < 0 ? tracks[tracks.Count - 1] : tracks[trackId - 1];
        Play(currentTrack);
    }

    public void Play(MusicTrack track)
    {
        audioSource.clip = track.audioClip;
        audioSource.Play();
        onTrackChanged.Invoke(track);

        if (timer != null)
        {
            StopCoroutine(timer);
        }

        float duration = currentTrack.audioClip.length;
        timer = StartCoroutine(SetTimeout(duration, () => {
            NextTrack();
        }));
    }

    public int GetTrackId(MusicTrack track)
    {
        return tracks.FindIndex(match => track.Equals(match));
    }

    IEnumerator SetTimeout(float duration, Action callback)
    {
        yield return new WaitForSeconds(duration);

        callback();
    }

    [System.Serializable]
    public class MusicPlayerEvent : UnityEvent<MusicTrack> { };
}
