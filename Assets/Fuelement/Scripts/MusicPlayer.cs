using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MusicPlayer {
    [RequireComponent(typeof(AudioSource))]
    public class MusicPlayer : Singleton<MusicPlayer>
    {
        public List<MusicTrack> tracks;
        [Space(10)]
        public MusicTrack currentTrack;
        public AudioSource audioSource;
        public bool shuffle = false;

        [Header("Controls")]
        public KeyCode prevTrackButton = KeyCode.LeftBracket;
        public KeyCode nextTrackButton = KeyCode.RightBracket;

        [Header("Events")]
        public MusicPlayerEvent onTrackChanged;

        Coroutine timer;

        void Start()
        {
            if (shuffle) ShuffleTrackList();
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

        void ShuffleTrackList()
        {
            List<MusicTrack> temp = new List<MusicTrack>(tracks);
            List<MusicTrack> newMusicTrackList = new List<MusicTrack>();

            while (temp.Count > 0)
            {
                int ind = UnityEngine.Random.Range(0, temp.Count);
                newMusicTrackList.Add(temp[ind]);
                temp.RemoveAt(ind);
            }

            tracks = newMusicTrackList;
        }

        public void NextTrack()
        {
            int trackId = GetTrackId(currentTrack);

            MusicTrack newTrack = trackId + 1 > tracks.Count - 1 ? tracks[0] : tracks[trackId + 1];
            Play(newTrack);
        }

        public void PreviousTrack()
        {
            int trackId = GetTrackId(currentTrack);

            MusicTrack newTrack = trackId - 1 < 0 ? tracks[tracks.Count - 1] : tracks[trackId - 1];
            Play(newTrack);
        }

        public void Play(MusicTrack track)
        {
            currentTrack = track;
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

        public void Pause()
        {
            audioSource.Pause();
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
}
