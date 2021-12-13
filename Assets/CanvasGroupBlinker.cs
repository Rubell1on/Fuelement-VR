using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CanvasGroupBlinker : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float delay = 1;
    public UnityEvent startedBlinking;
    public UnityEvent finishedBlinking;

    public void Blink(int count = 3)
    {
        StartCoroutine(_Blink(count, delay, true));
    }

    IEnumerator _Blink(int count, float delay, bool appears)
    {
        int blinked = 0;

        startedBlinking?.Invoke();

        while (blinked < count)
        {
            canvasGroup.alpha = 1;
            yield return new WaitForSeconds(delay);
            canvasGroup.alpha = 0;
            yield return new WaitForSeconds(delay);

            blinked++;
        }

        if (appears)
        {
            canvasGroup.alpha = 1;
        }

        finishedBlinking?.Invoke();
    }
}
