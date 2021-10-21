public class BreakPedalBehavior : DriverBehavior
{
    public AxisCarController controller;

    public void Start()
    {
        behaviorName = "Нажатие педали тормоза";
    }

    public override void FixedUpdate()
    {
        CurrentValue = controller.brake;
        CheckMissapplication();
    }
}
