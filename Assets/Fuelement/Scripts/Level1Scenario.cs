using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1Scenario : MonoBehaviour
{
    public enum ScenarioState { NotStarted, Started, Finished };
    public ScenarioState state;

    public CarScriptsController car;

    public level1StartUI startUI;
    public GameObject finishUI;

    public void Start()
    {
        startUI.gameObject.SetActive(true);
        DashBoard carDB = car.dashBoard;
        carDB.enabled = false;

        startUI.button.onClick.AddListener(() =>
        {
            carDB.enabled = true;
        });
    }

    public void StartScenario()
    {
        state = ScenarioState.Started;
        
    }

    public void FinishScenario()
    {
        state = ScenarioState.Finished;
        car.dashBoard.enabled = false;
        finishUI.SetActive(true);
    }
}
