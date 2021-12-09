using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineStart : MonoBehaviour
{
    public Drivetrain drivetrain;
    public AudioSource source;
    public AudioClip starter;
    public AudioClip started;

    public string axisName = "StartEngine";

    bool engineStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (drivetrain.rpm == 0)
        {
            engineStarted = false;
        }

        if (!engineStarted)
        {
            if (Input.GetAxis(axisName) == 1 || Input.GetAxis($"Keyboard_{axisName}") == 1)
            {
                if (drivetrain.startEngine && drivetrain.rpm < drivetrain.minRPM)
                {
                    if (!source.isPlaying)
                    {
                        source.clip = starter;
                        source.Play();
                    }
                }

                if(drivetrain.engineStarted)
                {
                    engineStarted = true;
                    source.clip = started;
                    source.Play();
                }
            }
        }
    }
}
