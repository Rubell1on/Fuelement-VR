using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class IdleController : MonoBehaviour
{
    public DynamicCameraController environmentCameraController;
    public FadeController fadeController;
    public List<int> fadeOnIds;

    [Space(10f)]
    [Header("Auto idle settings")]
    public bool autoIdle = false;
    public bool canExit = true;
    public float timeBeforeIdle = 10f;
    [Space(5)]
    public bool idleEnabled = false;
    [Space(10f)]
    [Header("Events")]
    public UnityEvent idleStarted;
    public UnityEvent idleStoped;

    Coroutine idleTimer = null;

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
        if (autoIdle && !idleEnabled)
            SetIdleTimer(timeBeforeIdle);
    }

    private void OnDestroy()
    {
        idleEnabled = false;
        ClearIdleTimer();
        StopIdle();
        environmentCameraController.StopAnimation();
    }

    [ContextMenu("Start idle")]
    public void StartIdle()
    {
        ClearIdleTimer();
        RemoveListeners();
        idleEnabled = true;
        environmentCameraController.movementFinished.AddListener(OnMovementFinished);
        fadeController.fadedIn.AddListener(OnStartFidedIn);
        fadeController.FadeIn();

        idleStarted?.Invoke();
    }

    [ContextMenu("Stop idle")]
    public void StopIdle()
    {
        idleEnabled = false;
        ClearIdleTimer();
        RemoveListeners();
        environmentCameraController.StopAnimation();
        fadeController.FadeOutInstantly();

        if (autoIdle && !idleEnabled)
        {
            SetIdleTimer(timeBeforeIdle);
        }

        idleStoped?.Invoke();
    }

    void RemoveListeners()
    {
        fadeController.fadedOut.RemoveListener(OnFadedOut);
        fadeController.fadedIn.RemoveListener(OnFadedIn);
        fadeController.fadedIn.RemoveListener(OnStartFidedIn);
        environmentCameraController.movementFinished.RemoveListener(OnMovementFinished);
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

    public void SetIdleTimer(float delay)
    {
        if (!idleEnabled && idleTimer == null)
        {
            idleTimer = StartCoroutine(_SetIdleTimer(delay));
        }
    }

    IEnumerator _SetIdleTimer(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartIdle();
    }

    public void ClearIdleTimer()
    {
        if (idleTimer != null)
        {
            StopCoroutine(idleTimer);
            idleTimer = null;
        }
    }

    private void Update()
    {
        List<float> values = inputs.Select(i => Input.GetAxis(i)).ToList().Where(i => i != 0).ToList();
        if (values.Count == 0)
            return;

        if (!idleEnabled && idleTimer != null)
        {
            ClearIdleTimer();
            SetIdleTimer(timeBeforeIdle);
            return;
        }

        if (canExit)
        {
            StopIdle();
        }
    }
}
