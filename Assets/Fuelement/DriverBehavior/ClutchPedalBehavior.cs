public class ClutchPedalBehavior : DriverBehavior
{
    public AxisCarController controller;

    public void Start()
    {
        behaviorName = "Нажатие педали сцепления";
    }

    public override void FixedUpdate()
    {
        CurrentValue = controller.clutchInput;
        CheckMissapplication();
    }
}