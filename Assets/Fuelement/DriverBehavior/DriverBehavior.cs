using System;
using UnityEngine;
using UnityEngine.Events;

public abstract class DriverBehavior : MonoBehaviour
{
    public float threshold = 10;
    public float currentValue = 0;
    public int errorsCount = 0;
    public bool debugInfo = false;

    public DriverBehaviorEvent ErrorOccurred;

    public virtual float CurrentValue { 
        get { return this.currentValue; }
        set { this.currentValue = value < 0 ? 0 : value; }
    }

    protected float prevValue = 0;
    protected float diffValue = 0;
    protected bool active = true;

    public virtual void FixedUpdate()
    {
        CheckMissapplication();
    }

    protected virtual void CheckMissapplication()
    {
        diffValue = Mathf.Abs(currentValue - prevValue);
        if (diffValue > threshold * Time.deltaTime)
        {
            if (active)
            {
                errorsCount++;

                string type = this.GetType().Name;
                ErrorOccurred.Invoke(new DriverBehaviorEventArgs(this, type, CurrentValue, diffValue));

                active = false;
            }
        }
        else
        {
            active = true;
        }

        if (debugInfo)
        {
            Debug.Log($"current: {currentValue} prev: {prevValue}");
        }
        
        prevValue = currentValue;
    }

    [Serializable]
    public class DriverBehaviorEvent : UnityEvent<DriverBehaviorEventArgs> { };

    [Serializable]
    public class DriverBehaviorEventArgs
    {
        [SerializeField] public object sender;
        [SerializeField] public string typeName;
        [SerializeField] public float currentValue;
        [SerializeField] public float diffValue;

        public DriverBehaviorEventArgs(object sender, string typeName, float currentValue, float diffValue)
        {
            this.sender = sender;
            this.typeName = typeName;
            this.currentValue = currentValue;
            this.diffValue = diffValue;
        }
    }
}
