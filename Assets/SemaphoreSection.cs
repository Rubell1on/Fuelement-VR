using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SemaphoreSection : MonoBehaviour
{
    public enum SectionType { none = -1, red, yellow, green };
    public SectionType type;
    public Color[] colors = { Color.red, Color.yellow, Color.red };
    public Color defaultColor = Color.black;
    public float emissionIntensity = 2;

    private Material _inner;

    private void Start()
    {
        _inner = GetComponent<MeshRenderer>().materials[1];
    }

    [ContextMenu("Set red")]
    public void SetRed()
    {
        SetColor(colors[(int)SectionType.red]);
    }

    [ContextMenu("Set yellow")]
    public void SetYellow()
    {
        SetColor(colors[(int)SectionType.yellow]);
    }

    [ContextMenu("Set green")]
    public void SetGreen()
    {
        SetColor(colors[(int)SectionType.green]);
    }

    [ContextMenu("Set default")]
    public void SetDefault()
    {
        SetColor(defaultColor);
        _inner.DisableKeyword("_EMISSION");
    }

    public void SetColor(Color color)
    {
        _inner.SetColor("_Color", color);
        _inner.EnableKeyword("_EMISSION");
        _inner.SetColor("_EmissionColor", color * emissionIntensity);
    }

    public void Blink(int count, float delay, Action callback = null)
    {
        StartCoroutine(_Blink(5, 0.5f, callback));
    }

    IEnumerator _Blink(int count, float delay, System.Action callback = null)
    {
        int blinked = 0;

        while(blinked < count)
        {
            SetDefault();
            yield return new WaitForSeconds(delay);
            SetColor(colors[(int)type]);
            yield return new WaitForSeconds(delay);
            blinked++;
        }

        if (callback != null)
            callback();
    }
}
