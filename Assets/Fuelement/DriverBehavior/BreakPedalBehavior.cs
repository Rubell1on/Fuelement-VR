public class BreakPedalBehavior : DriverBehavior
{
    public AxisCarController controller;

    public override void FixedUpdate()
    {
        CurrentValue = controller.brake;
        CheckMissapplication();
    }
}
