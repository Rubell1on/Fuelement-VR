using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.VectorGraphics;

public class TaskElement : MonoBehaviour
{
    public enum TaskState { Active, Success, Failed };
    public TaskState taskState;

    [SerializeField]
    private SVGImage icon;
    [SerializeField]
    private Text text;
    [SerializeField]
    private LayoutElement layoutElement;
    [SerializeField]
    private CanvasGroup canvasGroup;
    [SerializeField]
    private List<Sprite> sprites;

    public int minimizeTextLength = 41;
    public float minimizeHeight = 49f;
    public float timeout = 0.5f;
    public float minimizationSpeed = 2;
    public AnimationCurve animationCurve;

    string sourceText;

    void Start()
    {
        sourceText = text.text;
    }

    [ContextMenu("Minimize")]
    public void Minimize(Action callback = null)
    {
        RectTransform rt = GetComponent<RectTransform>();
        float sourceHeight = rt.sizeDelta.y;
        StartCoroutine(_Minimize(sourceHeight, minimizeHeight, () => {
            text.text = MinimizeText(text.text);

            if (callback != null)
                callback();
        }));
    }

    [ContextMenu("Maximize")]
    public void Maximize()
    {
        layoutElement.preferredHeight = -1;
        text.text = sourceText;
    }

    IEnumerator _Minimize(float sourceHeight, float targetHeight, Action callback = null)
    {        
        if (sourceHeight > minimizeHeight)
        {
            float time = 0;

            while (time < 0.99)
            {
                sourceHeight = Mathf.Lerp(sourceHeight, targetHeight, time);
                layoutElement.preferredHeight = sourceHeight;
                time += Time.deltaTime * minimizationSpeed;

                yield return new WaitForEndOfFrame();
            }
        }
        else
            yield return new WaitForSeconds(timeout);

        if (callback != null)
            callback();
    }

    public void SetText(string text)
    {
        if (this.text == null) return;

        this.text.text = text;
    }

    public void SetIcon(TaskState state)
    {
        if (icon == null) return;

        icon.sprite = sprites[(int)state];
    }

    public void SetOpacity(float targetOpacity = 0.5f, Action callback = null)
    {
        float opacity = canvasGroup.alpha;
        StartCoroutine(_SetOpacity(opacity, targetOpacity, callback));
    }

    IEnumerator _SetOpacity(float opacity, float targetOpacity, Action callback = null)
    {
        float time = 0;

        while(time < 0.99)
        {
            opacity = Mathf.Lerp(opacity, targetOpacity, time);
            canvasGroup.alpha = opacity;
            time += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        canvasGroup.alpha = targetOpacity;

        if (callback != null)
            callback();
    }

    string MinimizeText(string text)
    {
        if (text.Length >= minimizeTextLength)
        {
            string substring = text.Substring(0, minimizeTextLength - 3);
            List<string> splitted = substring.Split(' ').ToList();
            IEnumerable<string> ranged = splitted.GetRange(0, splitted.Count - 1);
            string targetString = String.Join(" ", ranged);
            return targetString.Insert(targetString.Length, "...");
        }

        return text;
    }
}
