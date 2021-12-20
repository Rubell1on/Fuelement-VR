using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelCard : MonoBehaviour
{
    public RawImage background;
    public Text title;
    public Text description;

    public void SetCardData(ASimpleData data)
    {
        SetBackground(data.background);
        SetTitle(data.title);
        SetDescription(data.description);
    }

    public void SetBackground(Texture texture)
    {
        if (background != null) background.texture = texture;
    }

    public void SetTitle(string text)
    {
        if (title != null) title.text = text;
    }

    public void SetDescription(string text)
    {
        if (description != null) description.text = text;
    }
}