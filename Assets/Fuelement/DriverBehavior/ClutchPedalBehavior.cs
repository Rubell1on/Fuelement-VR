public class ClutchPedalBehavior : DriverBehavior
{
    public AxisCarController controller;

    public override void FixedUpdate()
    {
        CurrentValue = controller.clutchInput;
        CheckMissapplication();
    }
}