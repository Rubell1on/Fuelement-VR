using System;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreenController : MonoBehaviour
{
    public Text levelName;
    public Text progress;
    public Text pressAnyKey;

    public void SetLevelName(string name)
    {
        if (levelName != null)
        {
            levelName.text = name;
        } else
        {
            throw new NullReferenceException("Text field levelName is not assign!");
        }
        
    }

    public void SetLoadingProgress(float value)
    {
        if (progress != null)
        {
            progress.text = $"Loading {value}%";
        }
        else
        {
            throw new NullReferenceException("Text field progress is not assign!");
        }
    }

    public void SetLoaded(bool value)
    {
        if (value)
        {
            pressAnyKey.gameObject.SetActive(true);
            progress.gameObject.SetActive(false);
        } else
        {
            pressAnyKey.gameObject.SetActive(false);
            progress.gameObject.SetActive(true);
        }
    }
}