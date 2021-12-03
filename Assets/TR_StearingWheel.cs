using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TR_StearingWheel : Phase
{
    public bool left = false;
    public bool right = false;

    [SerializeField]
    float stearingValue = 0;

    public override void StartPhase()
    {
        base.StartPhase();
        Debug.Log("Крутите рулевое колесо влево и вправо до конца");
    }

    public override void ResetValues()
    {
        left = false;
        right = false;
        stearingValue = 0;
    }

    void Update()
    {
        float value = Input.GetAxis("Horizontal");
        float kbValue = Input.GetAxis("Keyboard_Horizontal");

        if (value != 0)
        {
            stearingValue = value;
        } 
        else if (kbValue != 0)
        {
            stearingValue = kbValue;
        }

        if (stearingValue == -1) left = true;
        if (stearingValue == 1) right = true;

        if (left && right)
        {
            finished?.Invoke();
        }
    }
}