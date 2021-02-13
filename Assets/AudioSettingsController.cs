using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

public class AudioSettingsController : MonoBehaviour
{
    public AudioMixer audioMixer;
    public List<SettingsField> settings;

    AudioMixerGroup[] groups;

    Dictionary<string, float> currValues;
    Dictionary<string, float> oldValues;

    public AudioSettingsControllerEvent onValueChanged;

    void Start()
    {
        groups = GetGroups();
        oldValues = GetValues(groups);

        foreach (SettingsField field in settings)
        {
            onValueChanged.AddListener((key, value) =>
            {
                if (field.fieldName == key)
                    field.value = value;
            });

            field.onValueChanged.AddListener(value =>
            {
                audioMixer.SetFloat(field.fieldName, value);
            });
        }
    }

    // Update is called once per frame
    void Update()
    {
        currValues = GetValues(groups);

        foreach (KeyValuePair<string, float> e in currValues)
        {
            float value;

            if (oldValues.TryGetValue(e.Key, out value))
            {
                if (e.Value != value)
                {
                    onValueChanged.Invoke(e.Key, e.Value);
                }
            }
        }

        oldValues = currValues;
    }

    public Dictionary<string, float> GetValues(AudioMixerGroup[] groups)
    {
        Dictionary<string, float> currValues = new Dictionary<string, float>();

        foreach (AudioMixerGroup g in groups)
        {
            float value;
            audioMixer.GetFloat(g.name, out value);
            currValues.Add(g.name, value);
        }

        return currValues;
    }

    public AudioMixerGroup[] GetGroups()
    {
        return audioMixer.FindMatchingGroups("Master");
    }

    [System.Serializable]
    public class AudioSettingsControllerEvent : UnityEvent<string, float> {}
}