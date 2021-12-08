using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TasksController : MonoBehaviour
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

    public int Count { get { return tasks.Count; } }

    [ContextMenu("Add")]
    public void _Add()
    {
        Add("Здесь должен быть текст", TaskElement.TaskState.Active);
    }

    public void Add(string text, TaskElement.TaskState state = TaskElement.TaskState.Active)
    {
        GameObject instance = Instantiate(template.gameObject, body.transform);
        TaskElement element = instance.GetComponent<TaskElement>();
        element.SetText(text);
        element.SetIcon(state);

        tasks.Add(element);
        audioSource.clip = added;
        audioSource.Play();
    }
}
