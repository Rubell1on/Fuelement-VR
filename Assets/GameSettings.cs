using System;
using UnityEngine;
using UnityEngine.Audio;

public class GameSettings : MonoBehaviour
{
    public enum MixerGroupType { Master, Car, Music, Environment, Misc };

    [Header("Audio settings")]
    public AudioMixer audioMixer;

    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    public void SetMasterVolume(float value)
    {
        _SetVolume(MixerGroupType.Master, value);
    }

    public void SetCarVolume(float value)
    {
        _SetVolume(MixerGroupType.Car, value);
    }

    public void SetMusicVolume(float value)
    {
        _SetVolume(MixerGroupType.Music, value);
    }

    public void SetEnvironmentVolume(float value)
    {
        _SetVolume(MixerGroupType.Environment, value);
    }

    public void SetMiscVolume(float value)
    {
        _SetVolume(MixerGroupType.Misc, value);
    }

    void _SetVolume(MixerGroupType type, float value)
    {
        string name = Enum.GetName(typeof(MixerGroupType), (int)type);
        audioMixer.SetFloat(name, value);
    }

}
