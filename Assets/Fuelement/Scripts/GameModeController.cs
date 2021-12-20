using System;
using UnityEngine;
using UnityEngine.Events;

public class GameModeController : MonoBehaviour
{
    public bool ready = false;

    public OnChangeStateEvent onChangeState;

    public void ChangeState(dynamic dynamic)
    {
        if (LevelsController.GetInstance()?.CurrentLevel != null && CarsController.GetInstance()?.CurrentCar != null)
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
            CustomLevel level = LevelsController.GetInstance()?.CurrentLevel;
            CarData car = CarsController.GetInstance()?.CurrentCar;

            StartCoroutine(LevelManager.GetInstance()?.Load(level, car));
        } else
        {
            throw new NullReferenceException("Level or Car data is not set!");
        }
    }

    [Serializable]
    public class OnChangeStateEvent : UnityEvent<bool> { };
}