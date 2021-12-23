using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelementVidController : MonoBehaviour
{
    public CanvasGroup wrapper;
    public Image background;
    public RectTransform logo;
    public AudioSource audioSource;
    public enum FulementVidEventType { onFinish };

    private LTSeq sequence = null;

    private void Start()
    {
        Launch();
    }

    public void Launch()
    {
        if (sequence != null) return;

        sequence = LeanTween.sequence();
        sequence
            .append(() => audioSource.Play())
            .append(FadeOut(0f))
            .append(FadeIn(3f))
            .append(() => background.enabled = false)
            .append(LeanTween.size(logo, new Vector2(270, 270), 5f))
            .append(FadeOut(0.5f))
            .append(() => LeanTween.dispatchEvent((int)FulementVidEventType.onFinish))
            .append(() => sequence = null);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            sequence.reset();
            sequence = null;

        }
    }

    public LTDescr FadeIn(float duration)
    {
        return LeanTween.alphaCanvas(wrapper, 1, duration);
    }

    public LTDescr FadeOut(float duration)
    {
        return LeanTween.alphaCanvas(wrapper, 0, duration).setEase(LeanTweenType.easeOutExpo);
    }
}
