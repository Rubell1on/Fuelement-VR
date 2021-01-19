using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelCard : MonoBehaviour
{
    public RawImage background;
    public Text title;
    public Text description;

    public void setLevelInfo(CustomLevel level)
    {
        //if (background != null) 
            background.texture = level.background;
        if (title != null) 
            title.text = level.title;
        if (description != null) 
            description.text = level.description;
    }
}
