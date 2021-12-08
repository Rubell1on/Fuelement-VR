using System;
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

    string sourceText;

    void Start()
    {
        sourceText = text.text;
    }

    [ContextMenu("Minimize")]
    public void Minimize()
    {
        layoutElement.preferredHeight = minimizeHeight;
        text.text = MinimizeText(text.text);
    }

    [ContextMenu("Maximize")]
    public void Maximize()
    {
        layoutElement.preferredHeight = -1;
        text.text = sourceText;
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

    public void SetOpacity(float opacity = 0.5f)
    {
        canvasGroup.alpha = opacity;
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
