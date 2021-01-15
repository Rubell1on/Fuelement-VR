﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayerWindowController : MonoBehaviour
{
    public GameObject canvas;
    public MusicPlayerWindow window;
    public float delay = 5;
    public float duration = 0.7f;
    public LeanTweenType easeType = LeanTweenType.easeInOutQuad;
    Vector3 targetPosition = new Vector3(-480, -240, 0);
    Vector3 outPosition = new Vector3(-805, -240, 0);
    GameObject createdWindow;

    public void CreateWindow(MusicTrack track)
    {
        if (createdWindow == null)
        {
            createdWindow = Instantiate(window.gameObject, canvas.transform);
            createdWindow.GetComponent<MusicPlayerWindow>().SetData(track);
            Show();
        }
        
    }

    void Show()
    {
        if (!LeanTween.isTweening(createdWindow))
        {
            LTSeq seq = LeanTween.sequence();
            seq
                .insert(LeanTween.moveLocal(createdWindow, outPosition, 0))
                .insert(LeanTween.moveLocal(createdWindow, targetPosition, duration).setEase(easeType))
                .append(() =>
                {
                    LTDescr descr = LeanTween.moveLocal(createdWindow, outPosition, duration).setEase(easeType);
                    descr.delay = delay;
                    descr.setOnComplete(() => Destroy(createdWindow));
                });
        }
    }
}
