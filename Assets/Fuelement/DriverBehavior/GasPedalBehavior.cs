public class GasPedalBehavior : DriverBehavior
{
    public AxisCarController controller;

    public void Start()
    {
        behaviorName = "Нажатие педали газа";
    }

    public override void FixedUpdate()
    {
        CurrentValue = controller.throttle;
        CheckMissapplication();
    }
}