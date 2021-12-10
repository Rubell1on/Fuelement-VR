using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Window : MonoBehaviour
{
    public bool showInstantly = true;
    public float animationDuration = 0.5f;
    public WindowControls windowControls;
    [SerializeField]
    private CanvasGroup canvasGroup;
    [SerializeField]
    private AnimationCurve appearing;
    [SerializeField]
    private AnimationCurve scaling;

    //[Space(10)]
    //[Header("Events")]
    //public UnityEvent windowWasShown;
    //public UnityEvent windowWasHidden;

    [ContextMenu("Show")]
    public void Show(bool instantly = false)
    {
        if (canvasGroup.alpha == 1)
        {
            return;
        }

        if (instantly)
        {
            ShowInstantly();
            return;
        }

        if (!gameObject.activeSelf)
        {
            canvasGroup.alpha = 0;
            gameObject.SetActive(true);
        }

        RectTransform rt = canvasGroup.GetComponent<RectTransform>();
        LeanTween.scale(rt, Vector3.one * 0.9f, 0);
        LeanTween.scale(rt, Vector3.one, animationDuration).setEase(scaling);
        LeanTween.alphaCanvas(canvasGroup, 1, animationDuration).setEase(appearing);
        //    .setOnComplete(() =>
        //{
        //    windowWasShown?.Invoke();
        //});
    }

    [ContextMenu("Show instantly")]
    void ShowInstantly()
    {
        canvasGroup.alpha = 1;
        gameObject.SetActive(true);
    }

    [ContextMenu("Hide")]
    public void Hide(bool instantly = false)
    {
        if (canvasGroup.alpha == 0)
        {
            return;
        }

        if (instantly)
        {
            HideInstantly();
            return;
        }

        RectTransform rt = canvasGroup.GetComponent<RectTransform>();
        LeanTween.scale(rt, Vector3.one * 0.9f, animationDuration).setEase(scaling);
        LeanTween.alphaCanvas(canvasGroup, 0, animationDuration).setEase(appearing).setOnComplete(() => 
        {
            //windowWasHidden?.Invoke();
            gameObject.SetActive(false);
        });
    }

    [ContextMenu("Hide instantly")]
    void HideInstantly()
    {
        canvasGroup.alpha = 0;
        gameObject.SetActive(false);
    }
}
