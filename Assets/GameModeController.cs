using System;
using UnityEngine;
using UnityEngine.Events;

public class GameModeController : MonoBehaviour
{
    public SelectorController selectorController;
    public CarSelectorController carSelectorController;

    public bool ready = false;

    public OnChangeStateEvent onChangeState;

    public void ChangeState(dynamic dynamic)
    {
        if (selectorController.currentSelection != null && carSelectorController.currentSelection != null)
        {
            ready = true;
        } else
        {
            ready = false;
        }

        onChangeState.Invoke(ready);
    }

    public void LoadLevel()
    {
        if (ready)
        {
            StartCoroutine(LevelManager.Load(selectorController.currentSelection, carSelectorController.currentSelection));
        } else
        {
            throw new NullReferenceException("Level or Car data is not set!");
        }
    }

    [Serializable]
    public class OnChangeStateEvent : UnityEvent<bool> { };
}