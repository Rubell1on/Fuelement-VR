using UnityEngine;

public class StearingWheelBehavior : DriverBehavior
{
    public AxisCarController controller;

    int wheelAxisMaxAngle = 540;

    public override void FixedUpdate()
    {
        CurrentValue = controller.steering * wheelAxisMaxAngle;
        CheckMissapplication();
    }
}