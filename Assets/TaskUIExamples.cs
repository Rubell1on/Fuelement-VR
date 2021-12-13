using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskUIExamples : MonoBehaviour
{
    public TasksController tasksController;

    public void AddActiveTask()
    {
        tasksController.Add("Активное упражнение", TaskElement.TaskState.Active);
    }

    public void AddFailedTask()
    {
        tasksController.Add("Невыполненное упражнение", TaskElement.TaskState.Failed);
    }

    public void AddSuccessTask()
    {
        tasksController.Add("Выполненное упражнение", TaskElement.TaskState.Success);
    }
}
