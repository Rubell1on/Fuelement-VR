using UnityEngine;

public class StearingWheelBehavior : DriverBehavior
{
    public AxisCarController controller;

    int wheelAxisMaxAngle = 540;
    //Coroutine wheelLoop = null;

    public override void FixedUpdate()
    {
        CurrentValue = controller.steering * wheelAxisMaxAngle;
        CheckMissapplication();
    }


    //void StartWheelLoop()
    //{
    //    if (wheelLoop != null) StopWheelLoop();
    //    wheelLoop = StartCoroutine(WheelLoop());
    //}

    //void StopWheelLoop()
    //{
    //    if (wheelLoop != null)
    //    {
    //        StopCoroutine(wheelLoop);
    //        wheelLoop = null;
    //    }
    //}

    //IEnumerator WheelLoop()
    //{
    //    yield return new WaitForEndOfFrame();
    //}
}