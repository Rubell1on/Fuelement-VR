using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskUIExamples : MonoBehaviour
{
    public TasksController tasksController;

    public void AddActiveTask()
    {
        tasksController.Add("�������� ����������", TaskElement.TaskState.Active);
    }

    public void AddFailedTask()
    {
        tasksController.Add("������������� ����������", TaskElement.TaskState.Failed);
    }

    public void AddSuccessTask()
    {
        tasksController.Add("����������� ����������", TaskElement.TaskState.Success);
    }
}
