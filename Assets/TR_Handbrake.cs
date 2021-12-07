using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TR_Handbrake : Phase
{
    public bool handbrake = false;
    public override void StartPhase()
    {
        base.StartPhase();
        Debug.Log("Нажмите пробел или поднимите стояночный тормоз чтобы затормозить");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Handbrake") == 1 || Input.GetAxis("Keyboard_Handbrake") == 1)
        {
            handbrake = true;
        }

        if (handbrake)
        {
            finished?.Invoke();
        }
    }
}
