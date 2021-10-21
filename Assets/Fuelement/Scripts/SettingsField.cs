using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingsField : Field
{
    public string fieldName;
    public Slider slider;
    public InputField input;

    public override void Start()
    {
        base.Start();
        slider.onValueChanged.AddListener(SetValue);
        input.onValueChanged.AddListener(s => SetValue(Convert.ToSingle(s)));
        onValueChanged.AddListener(SetValue);
    }

    public void SetValue(float value)
    {
        this.value = value;
        slider.value = value;
        input.text = value.ToString();
    }
}