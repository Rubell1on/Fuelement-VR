using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SemaphoreSection : MonoBehaviour
{
    public bool Enabled { get { return enabled; } }
    public enum SectionType { red, yellow, green };
    public SectionType type;
    public Color[] colors = { Color.red, Color.yellow, Color.green };
    public Color disabled = Color.black;
    public float emissionIntensity = 2;

    private Material _inner;
    private bool enabled = false;

    private void Start()
    {
        _inner = GetComponent<MeshRenderer>().materials[1];

    }

    public void EnableLight()
    {
        if (enabled)
        {
            return;
        }

        SetColor(colors[(int)type]);
        enabled = true;
    }

    public void DisableLight()
    {
        if (!enabled)
        {
            return;
        }

        SetColor(disabled);
        _inner.DisableKeyword("_EMISSION");
        enabled = false;
    }

    public void Blink(int count = 3, float delay = 0.5f, Action callback = null)
    {
        StartCoroutine(_Blink(count, delay, callback));
    }

    void SetColor(Color color)
    {
        _inner.SetColor("_Color", color);
        _inner.EnableKeyword("_EMISSION");
        _inner.SetColor("_EmissionColor", color * emissionIntensity);
    }

    IEnumerator _Blink(int count, float delay, System.Action callback = null)
    {
        int blinked = 0;

        while(blinked < count)
        {
            DisableLight();
            yield return new WaitForSeconds(delay);
            EnableLight();
            yield return new WaitForSeconds(delay);
            blinked++;
        }

        if (callback != null)
            callback();
    }
}
