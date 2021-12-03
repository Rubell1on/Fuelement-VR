using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioController : Singleton<ScenarioController>
{
    public List<Phase> phases;
    public int currentPhaseId = 0;

    public Phase currentPhase { get { return phases[currentPhaseId]; } }
    // Start is called before the first frame update

    private new void Awake()
    {
        SetDoNotDestroyOnLoad(false);
        base.Awake();
        DisableAllPhases();
        StartPhase(currentPhaseId);
    }

    public void StartPhase(int phaseId)
    {
        if (phases.Count > 0 && phaseId <= phases.Count - 1)
        {
            currentPhaseId = phaseId;
            Phase phase = currentPhase;
            phase.enabled = true;
            phase.StartPhase();
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
}
