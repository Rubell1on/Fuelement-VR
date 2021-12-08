using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TasksController : Singleton<TasksController>
{
    [SerializeField]
    List<TaskElement> tasks = new List<TaskElement>();
    [SerializeField]
    RectTransform body;
    [SerializeField]
    TaskElement template;
    [Space(10)]
    [Header("Audio settings")]
    [SerializeField]
    AudioClip added;
    [SerializeField]
    AudioClip success;
    [SerializeField]
    AudioClip error;
    //List<AudioClip> audio = new List<AudioClip>();
    [SerializeField]
    AudioSource audioSource;
    public TaskElement currentTask { get { return tasks[tasks.Count - 1]; } }

    private new void Awake()
    {
        SetDoNotDestroyOnLoad(false);
        base.Awake();
    }

    [ContextMenu("Add")]
    public void _Add()
    {
        Add("Здесь должен быть текст", TaskElement.TaskState.Active);
    }

    public TaskElement Add(string text, TaskElement.TaskState state = TaskElement.TaskState.Active)
    {
        GameObject instance = Instantiate(template.gameObject, body.transform);
        TaskElement element = instance.GetComponent<TaskElement>();
        element.SetText(text);
        element.SetIcon(state);

        tasks.Add(element);
        audioSource.clip = added;
        audioSource.Play();

        return element;
    }

    public void FinishTask(TaskElement task, TaskElement.TaskState state)
    {
        task.SetIcon(state);
        task.Minimize();
        audioSource.clip = state == TaskElement.TaskState.Success ? success : error;
        audioSource.Play();
    }
}
