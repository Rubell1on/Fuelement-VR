using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StearingWheelBehavior : MonoBehaviour
{
    public AxisCarController controller;
    public float threshold = 10;
    public float currentAngle = 0;
    public int errorsCount = 0; 

    int wheelAxisMaxAngle = 540;
    float prevAngle = 0;
    bool active = true;
    //Coroutine wheelLoop = null;

    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        currentAngle = controller.steering * wheelAxisMaxAngle;
        if (Mathf.Abs(currentAngle - prevAngle) > threshold * Time.deltaTime)
        {
            if (active)
            {
                errorsCount++;
                active = false;
            }
        } else
        {
            active = true;
        }

        Debug.Log($"current: {currentAngle} prev: {prevAngle}");
        prevAngle = currentAngle;
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
