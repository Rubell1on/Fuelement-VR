using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TR_GearShifting : Phase
{
    public bool shiftUp = false;
    public bool shiftDown = false;

    bool clutchDisengaged = false;
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
        base.ResetValues();
        shiftUp = false;
        shiftDown = false;
        clutchDisengaged = false;
    }

    private void Update()
    {
        float clutchValue = Input.GetAxis("Clutch");
        float kbClutchValue = Input.GetAxis("Keyboard_Clutch");

        if (clutchValue == 1 || kbClutchValue == 1) clutchDisengaged = true;

        if (!shiftUp && clutchDisengaged && (Input.GetAxisRaw("ShiftUp") == 1 || Input.GetAxisRaw("Keyboard_ShiftUp") == 1))
        {
            shiftUp = true;
        }

        if (!shiftDown && clutchDisengaged && (Input.GetAxisRaw("ShiftDown") == 1 || Input.GetAxisRaw("Keyboard_ShiftDown") == 1))
        {
            shiftDown = true;
        }

        if (shiftUp && shiftDown && !executed)
        {
            executed = true;
            tasksController.FinishTask(task, TaskElement.TaskState.Success, () =>
            {
                finished?.Invoke(PhaseResult.Success);
            });
        }
    }
}
