using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class CustomButton : Button, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Space(10)]
    [Header("Audio settings")]
    [SerializeField]
    private AudioSource source;
    [SerializeField]
    private AudioClip pointerEnter;
    [SerializeField]
    private AudioClip pointerExit;
    [SerializeField]
    private AudioClip pointerClick;

    [Space(10)]
    [Header("Events")]
    public UnityEvent onPointerEnter;
    public UnityEvent onPointerExit;
    public CustomForm form;

    public override void OnSubmit(BaseEventData eventData)
    {
        base.OnSubmit(eventData);
        FormCheck(form);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        FormCheck(form);
        if (source != null && pointerClick != null)
        {
            source.clip = pointerClick;
            source.Play();
        }
    }

    public override void OnPointerEnter(PointerEventData pointerEventData)
    {
        base.OnPointerEnter(pointerEventData);
        if (source != null && pointerEnter != null)
        {
            source.clip = pointerEnter;
            source.Play();
        }

        onPointerEnter.Invoke();
    }

    public override void OnPointerExit(PointerEventData pointerEventData)
    {
        base.OnPointerExit(pointerEventData);
        if (source != null && pointerExit != null)
        {
            source.clip = pointerExit;
            source.Play();
        }

        onPointerExit.Invoke();
    }

    private void FormCheck(CustomForm form)
    {
        if (form != null)
        {
            form.Validate.Invoke();
        }
    }
}
