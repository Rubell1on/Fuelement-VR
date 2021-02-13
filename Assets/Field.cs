using System;
using UnityEngine;
using UnityEngine.Events;

public class Field : MonoBehaviour
{
    public float value;
    float prevValue;

    public FieldFloatEvent onValueChanged;

    public virtual void Start()
    {
        prevValue = value;
    }

    public virtual void Update()
    {
        if (!prevValue.Equals(value))
        {
            prevValue = value;
            onValueChanged.Invoke(value);
        }
    }

    [Serializable]
    public class FieldFloatEvent : UnityEvent<float> { }
}
