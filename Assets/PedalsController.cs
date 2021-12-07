using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedalsController : MonoBehaviour
{
    public Rotator clutch;
    public Rotator brake;
    public Rotator throttle;

    void Update()
    {
        if (clutch != null)
        {
            PressPedal("Clutch", clutch);
        }

        if (brake != null)
        {
            PressPedal("Brake", brake);
        }

        if (throttle != null)
        {
            PressPedal("Throttle", throttle);
        }
    }

    void PressPedal(string inputAxis, Rotator rotator)
    {
        float value = InputExtension.GetAxis(inputAxis);
        rotator.Rotate(value, rotator.targetAngle);
    }
}