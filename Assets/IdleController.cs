using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleController : MonoBehaviour
{
    public DynamicCameraController carCameraController;
    public DynamicCameraController environmentCameraController;
    public FadeController fadeController;
    public List<int> fadeOnIds;

    [Space(10f)]
    [Header("Auto idle settings")]
    public bool autoIdle = false;
    public bool canExit = true;
    public float timeBeforeIdle = 10f;
    [Space(5)]
    public bool autoIdleEnabled = false;

    Coroutine idle = null;

    List<string> inputs = new List<string>()
    {
        "Clutch",
        "Brake",
        "Throttle",
        "Horizontal",
        "StartEngine",
        "Handbrake",
        "ShiftUp",
        "ShiftDown",
        "Keyboard_Clutch",
        "Keyboard_Brake",
        "Keyboard_Throttle",
        "Keyboard_Horizontal",
        "Keyboard_StartEngine",
        "Keyboard_Handbrake",
        "Keyboard_ShiftUp",
        "Keyboard_ShiftDown"
    };

    private void Start()
    {
       // SetIdleTimer(timeBeforeIdle);
    }

    private void OnDestroy()
    {
        StopIdle();
    }

    [ContextMenu("Start idle")]
    public void StartIdle()
    {
        StopIdle();
        autoIdleEnabled = true;
        environmentCameraController.movementFinished.AddListener(OnMovementFinished);
        fadeController.fadedIn.AddListener(OnStartFidedIn);
        fadeController.FadeIn();
    }

    [ContextMenu("Stop idle")]
    public void StopIdle()
    {
        autoIdleEnabled = false;
        ClearIdleTimer();
        fadeController.fadedOut.RemoveListener(OnFadedOut);
        fadeController.fadedIn.RemoveListener(OnFadedIn);
        fadeController.fadedIn.RemoveListener(OnStartFidedIn);
        environmentCameraController.movementFinished.RemoveListener(OnMovementFinished);
        environmentCameraController.StopAnimation();

        //if (autoIdle && !autoIdleEnabled)
        //{
        //    SetIdleTimer(timeBeforeIdle);
        //    autoIdle = false;
        //}
    }

    void OnStartFidedIn()
    {
        fadeController.fadedIn.RemoveListener(OnStartFidedIn);
        environmentCameraController.MoveToInstant(0);
        fadeController.FadeOut();
    }

    void OnMovementFinished(int id)
    {
        if (fadeOnIds.Contains(id))
        {
            fadeController.fadedIn.AddListener(OnFadedIn);
            fadeController.FadeIn();
        }
        else
        {
            environmentCameraController.Next();
        } 
    }

    void OnFadedIn()
    {
        fadeController.fadedIn.RemoveListener(OnFadedIn);
        fadeController.fadedOut.AddListener(OnFadedOut);
        environmentCameraController.NextInstant();
        fadeController.FadeOut();
    }

    void OnFadedOut()
    {
        fadeController.fadedOut.RemoveListener(OnFadedOut);
    }

    void SetIdleTimer(float delay)
    {
        if (idle != null) StopCoroutine(idle);
        idle = StartCoroutine(_SetIdleTimer(delay));
    }

    IEnumerator _SetIdleTimer(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartIdle();
    }

    void ClearIdleTimer()
    {
        if (idle != null) StopCoroutine(idle);
        autoIdle = true;
    }

    private void Update()
    {
        if (canExit)
        {
            List<float> values = inputs.Select(i => Input.GetAxis(i)).ToList().Where(i => i != 0).ToList();
            if (values.Count > 0)
            {
                StopIdle();
            }
        }
    }
}
