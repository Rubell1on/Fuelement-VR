using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class FadeController : MonoBehaviour
{
    public Image image;
    public AnimationCurve animationCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0, 0), new Keyframe(1, 1) });
    public float speed = 1;
    public float lerpThreshold = 0.99f;
    public UnityEvent fadedIn;
    public UnityEvent fadedOut;

    [ContextMenu("Fade In")]
    public void FadeIn()
    {
        StartCoroutine(_FadeIn());
    }

    [ContextMenu("Fade Out")]
    public void FadeOut()
    {
        StartCoroutine(_FadeOut());
    }

    IEnumerator _FadeIn()
    {
        float time = 0;
        Color currentColor = image.color;
        Color targetColor = currentColor;
        targetColor.a = 1;

        while(time <= lerpThreshold)
        {
            float value = animationCurve.Evaluate(time);
            image.color = Color.Lerp(currentColor, targetColor, value);
            time += Time.deltaTime * speed;

            if (!image.color.Equals(targetColor))
                yield return new WaitForEndOfFrame();
        }

        image.color = targetColor;

        fadedIn?.Invoke();
    }

    IEnumerator _FadeOut()
    {
        float time = 0;
        Color currentColor = image.color;
        Color targetColor = currentColor;
        targetColor.a = 0;

        while (time <= lerpThreshold)
        {
            float value = animationCurve.Evaluate(time);
            image.color = Color.Lerp(currentColor, targetColor, value);
            time += Time.deltaTime * speed;

            if (!image.color.Equals(targetColor))
                yield return new WaitForEndOfFrame();
        }

        image.color = targetColor;

        fadedOut?.Invoke();
    }
}
