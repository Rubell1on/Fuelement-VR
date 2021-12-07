using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedalsController : MonoBehaviour
{
    public Rotatator clutch;
    public Rotatator brake;
    public Rotatator throttle;

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

    void PressPedal(string inputAxis, Rotatator rotator)
    {
        float value = InputExtension.GetAxis(inputAxis);
        rotator.Rotate(value, rotator.targetAngle);
    }
}