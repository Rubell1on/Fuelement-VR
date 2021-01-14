using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelementVidController : MonoBehaviour
{
    public CanvasGroup wrapper;
    public RectTransform logo;
    public AudioSource audioSource;
    public enum FulementVidEventType { onFinish };

    public void Launch()
    {
        LTSeq seq = LeanTween.sequence();
        seq.append(FadeOut(0f))
            .append(FadeIn(3f))
            .append(LeanTween.size(logo, new Vector2(270, 270), 5f))
            .append(FadeOut(0.5f))
            .append(() => LeanTween.dispatchEvent((int)FulementVidEventType.onFinish));

        audioSource.Play();
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
