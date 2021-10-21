using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarPlacementRotation : MonoBehaviour
{
    public float duration = 2f;
    public float fps { get { return 1 / Time.deltaTime; } }

    private void Start()
    {
        StartCoroutine(RotateY());
    }

    IEnumerator RotateY()
    {
        RectTransform rt = gameObject.GetComponent<RectTransform>();

        while (true)
        {
            float y = rt.rotation.eulerAngles.y;

            yield return new WaitForEndOfFrame();

            float newY = y - (duration * 1 * Time.deltaTime);
            rt.Rotate(Vector3.up, newY - y);
        }
    }

    IEnumerator RotateY(float target)
    {
        RectTransform rt = GetComponent<RectTransform>();

        float startY = rt.rotation.eulerAngles.y;
        float diff = target - startY;


        while (Mathf.Abs(target - rt.rotation.eulerAngles.y) > 0.1)
        {
            yield return new WaitForEndOfFrame();
            float step = diff / (duration * fps);
            rt.Rotate(Vector3.up, step);
        }
    }
}
