using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TR_StearingWheel : Phase
{
    public bool left = false;
    public bool right = false;

    [SerializeField]
    float stearingValue = 0;
    TasksController tasksController;
    TaskElement task;

    public override void StartPhase()
    {
        base.StartPhase();
        tasksController = TasksController.GetInstance();
        task = tasksController?.Add(info);
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
            tasksController.FinishTask(task, TaskElement.TaskState.Success);
            finished?.Invoke(PhaseResult.Success);
        }
    }
}