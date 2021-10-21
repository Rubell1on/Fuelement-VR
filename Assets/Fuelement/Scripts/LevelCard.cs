using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelCard : MonoBehaviour
{
    public RawImage background;
    public Text title;
    public Text description;

    public void setCardData(ASimpleData data)
    {
        if (background != null) background.texture = data.background;
        if (title != null) title.text = data.title;
        if (description != null) description.text = data.description;
    }

    public void setBackground(Texture texture)
    {
        if (background != null) background.texture = texture;
    }

    public void setTitle(string text)
    {
        if (title != null) title.text = text;
    }

    public void setDescription(string text)
    {
        if (description != null) description.text = text;
    }
}