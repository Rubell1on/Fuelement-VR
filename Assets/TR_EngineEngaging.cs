using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TR_EngineEngaging : Phase
{
    public bool engineStarted = false;
    public override void StartPhase()
    {
        base.StartPhase();
        Debug.Log("Запустите двигатель автомобиля удерживая латинскую клавишу 'E'");
    }

    public override void ResetValues()
    {
        engineStarted = false;
    }

    private void Update()
    {
        if (Input.GetAxis("StartEngine") == 1 || Input.GetAxis("Keyboard_StartEngine") == 1) engineStarted = true;

        if (engineStarted)
        {
            finished?.Invoke();
        }
    }
}
