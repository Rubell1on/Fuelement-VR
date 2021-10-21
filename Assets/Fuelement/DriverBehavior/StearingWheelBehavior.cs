using UnityEngine;

public class StearingWheelBehavior : DriverBehavior
{
    public void Start()
    {
        behaviorName = "Поворот рулевого колеса";    
    }

    public AxisCarController controller;

    int wheelAxisMaxAngle = 540;

    public override void FixedUpdate()
    {
        CurrentValue = controller.steering * wheelAxisMaxAngle;
        CheckMissapplication();
    }
}