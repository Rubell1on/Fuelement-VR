using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TR_EngineEngaging : Phase
{
    public bool engineStarted = false;
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
        engineStarted = false;
    }

    private void Update()
    {
        if (Input.GetAxis("StartEngine") == 1 || Input.GetAxis("Keyboard_StartEngine") == 1) engineStarted = true;

        if (engineStarted)
        {
            tasksController.FinishTask(task, TaskElement.TaskState.Success);
            finished?.Invoke(PhaseResult.Success);
        }
    }
}
