using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearShifterController : MonoBehaviour
{
    public Animator animator;
    public Drivetrain drivetrain;

    int previousShift = 0;

    // Update is called once per frame
    void Update()
    {
        int currentShift = drivetrain.gear;

        if (previousShift != currentShift)
        {
            previousShift = currentShift;
            animator.SetInteger("CurrentShift", currentShift);
        }
        
    }
}
