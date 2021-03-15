public class GasPedalBehavior : DriverBehavior
{
    public AxisCarController controller;

    public override void FixedUpdate()
    {
        CurrentValue = controller.throttle;
        CheckMissapplication();
    }
}