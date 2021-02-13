using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Audio;

public class GameSettings : MonoBehaviour
{
    const string settingsFolder = "Fuelement/Settings";
    public string settingsPath {
        get { return $"{Application.dataPath}/{settingsFolder}/settings.json"; }
    }

    [Header("Audio settings")]
    public AudioMixer audioMixer;

    private void Start()
    {
        DontDestroyOnLoad(this);
        //SaveSettings();
        //LoadSettings();
    }

    public void SetMasterVolume(float value)
    {
        _SetVolume(AudioSettings.MixerGroupType.Master, value);
    }

    public void SetCarVolume(float value)
    {
        _SetVolume(AudioSettings.MixerGroupType.Car, value);
    }

    public void SetMusicVolume(float value)
    {
        _SetVolume(AudioSettings.MixerGroupType.Music, value);
    }

    public void SetEnvironmentVolume(float value)
    {
        _SetVolume(AudioSettings.MixerGroupType.Environment, value);
    }

    public void SetMiscVolume(float value)
    {
        _SetVolume(AudioSettings.MixerGroupType.Misc, value);
    }

    void _SetVolume(AudioSettings.MixerGroupType type, float value)
    {
        string name = EnumToString(type);
        audioMixer.SetFloat(name, value);
    }

    void _SetVolume(string key, float value)
    {
        if (AudioSettings.ContainsTypeKey(key))
        {
            audioMixer.SetFloat(key, value);
        }
    }

    string EnumToString(AudioSettings.MixerGroupType type)
    {
        return Enum.GetName(typeof(AudioSettings.MixerGroupType), (int)type);
    }

    public void SaveSettings()
    {
        AudioSettings audioSettings = _SaveAudioSettings();

        Settings settings = new Settings(audioSettings);
        string settingsString = JsonUtility.ToJson(settings);

        File.WriteAllText(settingsPath, settingsString);
    }

    public void LoadSettings()
    {
        if (File.Exists(settingsPath))
        {
            string settingsStrings = File.ReadAllText(settingsPath);
            Settings settings = JsonUtility.FromJson<Settings>(settingsStrings);

            _LoadAudioSettings(settings.audio);
        } else
        {
            Debug.LogError($"Settings file in '{settingsPath}' not found");
        }
    }

    AudioSettings _SaveAudioSettings()
    {
        AudioSettings audioDict = new AudioSettings();
        string[] names = Enum.GetNames(typeof(AudioSettings.MixerGroupType));

        foreach (string n in names)
        {
            float value;
            audioMixer.GetFloat(n, out value);

            audioDict.Add(n, value);
        }

        return audioDict;
    }

    void _LoadAudioSettings(AudioSettings audio)
    {
        foreach (string k in audio.Keys)
        {
            if (AudioSettings.ContainsTypeKey(k))
            {
                float v;
                if (audio.TryGetValue(k, out v))
                {
                    _SetVolume(k, v);
                }
                else
                {
                    Debug.LogError($"An error occurred while parsing '{k}' key of audio settings");
                }
                
            }
        }
        Debug.Log("");
    }

    [Serializable]
    public class Settings
    {
        [SerializeField]
        public AudioSettings audio;

        public Settings(AudioSettings audio)
        {
            this.audio = audio;
        }
    }
}

[Serializable]
public class AudioSettings : SerializableDictionary<string, float>
{
    public enum MixerGroupType { Master, Car, Music, Environment, Misc };

    public static bool ContainsTypeKey(string key)
    {
        return _GetNames().Contains(key);
    }

    static List<string> _GetNames()
    {
        return Enum.GetNames(typeof(MixerGroupType)).ToList();
    }
}
