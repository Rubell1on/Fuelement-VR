using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TR_Pedals : Phase
{
    public bool throttlePedal;
    public bool breakPedal;
    public bool clutchPedal;

    public override void StartPhase()
    {
        base.StartPhase();
        Debug.Log("По очереди нажмите каждую из трех педалей");
    }

    public override void ResetValues()
    {
        throttlePedal = false;
        breakPedal = false;
        clutchPedal = false;
    }

    void Update()
    {
        if (!throttlePedal)
        {
            float throttleValue = Input.GetAxis("Throttle");
            float kbThrottleValue = Input.GetAxis("Keyboard_Throttle");

            if (throttleValue == 1 || kbThrottleValue == 1)
            {
                throttlePedal = true;
            }
        }

        if (!breakPedal)
        {
            float breakValue = Input.GetAxis("Brake");
            float kbBreakValue = Input.GetAxis("Keyboard_Brake");

            if (breakValue == 1 || kbBreakValue == 1)
            {
                breakPedal = true;
            }
        }

        if (!clutchPedal)
        {
            float clutchValue = Input.GetAxis("Clutch");
            float kbClutchValue = Input.GetAxis("Keyboard_Clutch");

            if (clutchValue == 1 || kbClutchValue == 1)
            {
                clutchPedal = true;
            }
        }

        if (throttlePedal && breakPedal && clutchPedal)
        {
            finished?.Invoke();
        }
    }
}

//public class Phase : MonoBehaviour
//{
//    public UnityEvent finished;
//    public UnityEvent failed;

//    public virtual void Enabled(bool value) 
//    {
//        this.enabled = value;
//    }

//    public void Restart() { }
//    public void OnFinished();
//    public void OnFailed();
//}

//public interface IPhase
//{
//    public void SetActive(bool value);
//    public void Reset();
//    public void OnFinished();
//    public void OnFailed();
//}
