using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class DriverBehavior : MonoBehaviour
{
    public string behaviorName;
    public float threshold = 10;
    public int errorsCount = 0;
    public float minValue = 100;
    public float maxValue = 0;
    public float avg = 0;
    public float diffValue = 0;
    public bool debugInfo = false;

    public DriverBehaviorEvent avgValueChanged;
    public DriverBehaviorEvent maxValueChanged;
    public DriverBehaviorEvent minValueChanged;
    public DriverBehaviorEvent ErrorOccurred;

    public virtual float CurrentValue { 
        get 
        {
            int count = values.Count;
            return count == 0 ? values[0] : values[count - 1];
        }
        set 
        { 
            float currentValue = value < 0 ? 0 : value;
            values.Add(currentValue);
            SetMaxValue(currentValue);
            SetMinValue(currentValue);
            SetAvg();
        }
    }

    protected List<float> values = new List<float>();
    protected float prevValue = 0;
    protected bool active = true;
    protected string type = "";

    public void Awake()
    {
        type = this.GetType().Name;
    }

    public virtual void FixedUpdate()
    {
        CheckMissapplication();
    }

    protected void SetAvg()
    {
        avg = values.Sum() / values.Count;
        avgValueChanged.Invoke(new DriverBehaviorEventArgs(this));
    }

    protected void SetMaxValue(float value)
    {
        if (value > maxValue)
        {
            maxValue = value;
            maxValueChanged.Invoke(new DriverBehaviorEventArgs(this));
        }
    }

    protected void SetMinValue(float value)
    {
        if (value < minValue)
        {
            minValue = value;
            minValueChanged.Invoke(new DriverBehaviorEventArgs(this));
        }
    }

    protected virtual void CheckMissapplication()
    {
        diffValue = Mathf.Abs(CurrentValue - prevValue);
        if (diffValue > threshold * Time.deltaTime)
        {
            if (active)
            {
                errorsCount++;

                ErrorOccurred.Invoke(new DriverBehaviorEventArgs(this));

                active = false;
            }
        }
        else
        {
            active = true;
        }

        if (debugInfo)
        {
            Debug.Log($"current: {CurrentValue} prev: {prevValue}");
        }
        
        prevValue = CurrentValue;
    }

    [Serializable]
    public class DriverBehaviorEvent : UnityEvent<DriverBehaviorEventArgs> { };

    [Serializable]
    public class DriverBehaviorEventArgs
    {
        [SerializeField] public object sender;

        public DriverBehaviorEventArgs(object sender) { this.sender = sender; }
    }
}
