using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class AchievementUI : MonoBehaviour
{
    public Vector2 startSize = new Vector2(100, 100);
    public Vector2 targetSize = new Vector2(512, 100);
    public float duration = 0.5f;
    public float delayBeforeHide = 10f;
    public Vector3 startScale = new Vector3(0.9f, 0.9f, 0.9f);
    public string Title { set { title.text = value; } }
    public string Description { set { description.text = value; } }
    public AnimationCurve easeIn = new AnimationCurve(new Keyframe[] { new Keyframe(0, 0), new Keyframe(1, 1) });

    [SerializeField]
    private Text title;
    [SerializeField]
    private Text description;
    private CanvasGroup canvasGroup;
    private RectTransform rt;
    [SerializeField]
    private CanvasGroup placeholder;
    [SerializeField]
    private CanvasGroup info;

    [ContextMenu("Show and hide")]
    public void ShowAndHide(Action callback = null)
    {
        LTSeq sequence = LeanTween.sequence();
        sequence
            .append(() => Show())
            .append(delayBeforeHide)
            .append(() => Hide(callback));
    }

    [ContextMenu("Show")]
    public void Show()
    {
        LeanTween.alphaCanvas(canvasGroup, 0, 0);
        LeanTween.size(rt, startSize, 0);
        LeanTween.scale(rt, startScale, 0);
        LeanTween.alphaCanvas(placeholder, 0, 0);
        LeanTween.alphaCanvas(info, 0, 0);

        LTSeq sequence = LeanTween.sequence();
        sequence
            .append(() =>
            {
                LeanTween.scale(rt, Vector3.one, duration).setEase(easeIn);
                LeanTween.alphaCanvas(canvasGroup, 1, duration).setEase(easeIn);
            })
            .append(1f)
            .append(() => LeanTween.size(rt, targetSize, duration).setEase(easeIn))
            .append(duration / 2)
            .append(() => LeanTween.alphaCanvas(placeholder, 1, duration).setEase(easeIn))
            .append(1f)
            .append(() => LeanTween.alphaCanvas(placeholder, 0, duration).setEase(easeIn))
            .append(duration)
            .append(() => LeanTween.alphaCanvas(info, 1, duration).setEase(easeIn));
    }

    [ContextMenu("Hide")]
    public void Hide(Action callback = null)
    {
        LTSeq sequence = LeanTween.sequence();
        sequence
            .append(() => LeanTween.alphaCanvas(info, 0, duration).setEase(easeIn))
            .append(1f)
            .append(() => LeanTween.size(rt, startSize, duration).setEase(easeIn))
            .append(1f)
            .append(() =>
            {
                LeanTween.scale(rt, startScale, duration).setEase(easeIn);
                LTDescr descr = LeanTween.alphaCanvas(canvasGroup, 0, duration).setEase(easeIn);
                if (callback != null)
                {
                    descr.setOnComplete(callback);
                }
            });
    }

    // Start is called before the first frame update
    void Awake()
    {
        rt = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
