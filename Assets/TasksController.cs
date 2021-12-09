using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TasksController : Singleton<TasksController>
{
    [SerializeField]
    Text title;
    [SerializeField]
    List<TaskElement> tasks = new List<TaskElement>();

    [Space(10)]
    [Header("Main settings")]
    [SerializeField]
    RectTransform body;
    [SerializeField]
    TaskElement template;

    [Space(10)]
    [Header("Animation settings")]
    [SerializeField]
    CanvasGroup canvasGroup;
    public AnimationCurve animationCurve;
    public float animationSpeed = 2;

    [Space(10)]
    [Header("Audio settings")]
    [SerializeField]
    AudioClip added;
    [SerializeField]
    AudioClip success;
    [SerializeField]
    AudioClip error;
    [SerializeField]
    AudioSource audioSource;

    Coroutine taskBar = null;
    public TaskElement currentTask { get { return tasks[tasks.Count - 1]; } }
    public string Title { get { return title.text; } set { title.text = value; } }

    private new void Awake()
    {
        SetDoNotDestroyOnLoad(false);
        base.Awake();
    }

    public TaskElement Add(string text, TaskElement.TaskState state = TaskElement.TaskState.Active)
    {
        audioSource.clip = added;
        audioSource.Play();
        GameObject instance = Instantiate(template.gameObject, body.transform);
        TaskElement element = instance.GetComponent<TaskElement>();
        element.SetText(text);
        element.SetIcon(state);

        tasks.Add(element);

        return element;
    }

    [ContextMenu("Remove all tasks")]
    public void RemoveAllTasks()
    {
        tasks.ForEach(RemoveTask);
        tasks.Clear(); 
    }
    public void RemoveTask(TaskElement task)
    {
        task.Minimize();
        task.SetOpacity(0, () =>
        {
            tasks.Remove(task);
            Destroy(task.gameObject);
        });
    }

    public void FinishTask(TaskElement task, TaskElement.TaskState state, Action callback)
    {
        audioSource.clip = state == TaskElement.TaskState.Success ? success : error;
        audioSource.Play();

        task.SetIcon(state);
        task.SetOpacity();
        task.Minimize(callback);
    }

    public void ShowTaskBarInstantly()
    {
        if (taskBar != null) StopCoroutine(taskBar);
        canvasGroup.alpha = 1;
    }

    public void HideTaskBarInstantly()
    {
        if (taskBar != null) StopCoroutine(taskBar);
        canvasGroup.alpha = 0;
    }

    [ContextMenu("Show task bar")]
    public void ShowTaskBar()
    {
        if (taskBar != null) StopCoroutine(taskBar);
        taskBar = StartCoroutine(_SetTaskBarOpacity(1));
    }

    [ContextMenu("Hide task bar")]
    public void HideTaskBar()
    {
        if (taskBar != null) StopCoroutine(taskBar);
        taskBar = StartCoroutine(_SetTaskBarOpacity(0));
    }

    IEnumerator _SetTaskBarOpacity(float targetOpacity)
    {
        float currentOpacity = canvasGroup.alpha;
        float time = 0;

        while(time < 0.99)
        {
            float value = animationCurve.Evaluate(time);
            canvasGroup.alpha = Mathf.Lerp(currentOpacity, targetOpacity, value);
            time += Time.deltaTime * animationSpeed;
            yield return new WaitForEndOfFrame();
        }

        canvasGroup.alpha = targetOpacity;
    }
}
