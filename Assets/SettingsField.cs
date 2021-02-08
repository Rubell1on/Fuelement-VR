using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingsField : MonoBehaviour
{
    public Slider slider;
    public InputField input;

    private void Start()
    {
        slider.onValueChanged.AddListener(v =>
        {
            input.text = v.ToString();
        });

        input.onEndEdit.AddListener(v =>
        {
            slider.value = Convert.ToSingle(v);
        });
    }
}
