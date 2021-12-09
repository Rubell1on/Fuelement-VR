﻿using System;
using UnityEngine;
using UnityEngine.Events;

public class Phase : MonoBehaviour
{
    public string info = "";
    public enum PhaseResult { Success, Failed };
    public bool executed = false;

    public UnityEvent started = new UnityEvent();
    public PhaseEvent finished = new PhaseEvent();

    public virtual void StartPhase() 
    {
        started?.Invoke();
    }

    [ContextMenu("Restart phase")]
    public virtual void RestartPhase()
    {
        ResetValues();
        StartPhase();
    }

    public virtual void ResetValues() {
        executed = false;
    }

    [Serializable]
    public class PhaseEvent : UnityEvent<PhaseResult> { }
}
