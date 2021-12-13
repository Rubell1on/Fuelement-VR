using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScenarioController : Singleton<ScenarioController>
{
    public List<Phase> phases;
    public int currentPhaseId = 0;

    public Phase currentPhase { get { return phases[currentPhaseId]; } }
    [Space(10)]
    [Header("Events")]
    public UnityEvent scenarioSetup;
    public UnityEvent scenarioStarted;
    public PhaseChanged phaseChanged;

    private new void Awake()
    {
        SetDoNotDestroyOnLoad(false);
        base.Awake();
        DisableAllPhases();
    }

    private void Start()
    {
        scenarioSetup?.Invoke();
    }

    public void StartScenario()
    {
        scenarioStarted?.Invoke();
        StartPhase(0);
    }

    public void StartPhase(int phaseId)
    {
        if (phases.Count > 0 && phaseId <= phases.Count - 1)
        {
            currentPhaseId = phaseId;
            Phase phase = currentPhase;
            phase.enabled = true;
            phase.StartPhase();
            phaseChanged?.Invoke(new PhaseEventArgs(phaseId, phase));
        }
    }

    public void DisableAllPhases()
    {
        phases.ForEach(p => p.enabled = false);
    }

    [ContextMenu("Restart scenario")]
    public void RestartScenario()
    {
        if (phases.Count > 0)
        {
            currentPhaseId = 0;
            phases.ForEach(p => p.ResetValues());
            DisableAllPhases();
            StartPhase(currentPhaseId);
        }
    }

    [ContextMenu("Next phase")]
    public void Next()
    {
        if (phases.Count == 0)
        {
            Debug.LogError("Массив фаз сценария пуст");
            return;
        }

        currentPhase.enabled = false;

        if (currentPhaseId < phases.Count - 1)
        {
            currentPhaseId++;
            StartPhase(currentPhaseId);
        }
    }

    public class PhaseChanged : UnityEvent<PhaseEventArgs> { }
    public class PhaseEventArgs
    {
        public int id;
        public Phase phase;

        public PhaseEventArgs(int id, Phase phase)
        {
            this.id = id;
            this.phase = phase;
        }
    }
}
