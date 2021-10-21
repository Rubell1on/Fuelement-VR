using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class CameraController : MonoBehaviour
{
    public Camera cam;
    public Camera vrCam;
    public bool vrEnabled = false;

    bool prevVREnabled;

    void Awake()
    {
        prevVREnabled = vrEnabled;
        SetVREnable(vrEnabled);
    }

    void Update()
    {
        if (prevVREnabled != vrEnabled)
        {
            SetVREnable(vrEnabled);
            prevVREnabled = vrEnabled;
        }
    }

    void SetVREnable(bool state)
    {
        if (state)
        {
            vrCam.gameObject.SetActive(true);
            cam.gameObject.SetActive(false);
        }
        else
        {
            cam.gameObject.SetActive(true);
            vrCam.gameObject.SetActive(false);
        }

        XRSettings.enabled = vrEnabled;
    }
}
