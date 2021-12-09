using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TR_Handbrake : Phase
{
    public bool handbrake = false;
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
        handbrake = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Handbrake") == 1 || Input.GetAxis("Keyboard_Handbrake") == 1)
        {
            handbrake = true;
        }

        if (handbrake && !executed)
        {
            executed = true;
            tasksController.FinishTask(task, TaskElement.TaskState.Success, () =>
            {
                finished?.Invoke(PhaseResult.Success);
            });
        }
    }
}
