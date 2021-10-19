using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClutchEngadingBehavior : DriverBehavior
{
    public override void FixedUpdate()
    {
        CurrentValue = Input.GetAxis("Clutch");
        CheckMissapplication();
    }

    protected override void CheckMissapplication()
    {
        float shiftUp = Input.GetAxisRaw("ShiftUp");
        float shiftDown = Input.GetAxisRaw("ShiftDown");

        if (CurrentValue <= threshold)
        {
            if (shiftUp == 1 || shiftDown == 1)
            {
                errorsCount++;

                string type = this.GetType().Name;
                ErrorOccurred.Invoke(new DriverBehaviorEventArgs(this, type, CurrentValue, 0));
            }
        }
    }
}
