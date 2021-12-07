using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DynamicCamera : MonoBehaviour
{
    public Camera sourceCamera;
    public Transform[] cameraPlaces;
    public int currentPlaceId = 0;
    public float speedAmplifier = 1;

    [Space(10f)]
    [Header("Animation settings")]
    public bool useCurve = true;
    public AnimationCurve animationCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0, 0), new Keyframe(1, 1) });
    [Space(10f)]
    [Header("Events")]
    public UnityEvent movementFinished;

    [ContextMenu("Next")]
    public void Next()
    {
        if (currentPlaceId + 1 <= cameraPlaces?.Length - 1)
        {
            currentPlaceId++;
            MoveTo(currentPlaceId);
        }
    }

    public void MoveTo(int id = 0)
    {
        if (id <= cameraPlaces?.Length - 1)
        {
            currentPlaceId = id;
            Transform target = cameraPlaces[id];
            StartCoroutine(MoveTo(target));
        }
    }

    IEnumerator MoveTo(Transform target, Action callback = null)
    {
        Vector3 currentPosition = sourceCamera.transform.position;
        Quaternion currentRotation = sourceCamera.transform.rotation;

        float lerpTime = 0;

        while (lerpTime < 1 * 0.99)
        {
            float value = lerpTime;
            value = animationCurve.Evaluate(lerpTime);
            currentPosition = Vector3.Lerp(currentPosition, target.position, value);
            currentRotation = Quaternion.Lerp(currentRotation, target.rotation, value);

            sourceCamera.transform.position = currentPosition;
            sourceCamera.transform.rotation = currentRotation;

            lerpTime += Time.deltaTime * speedAmplifier;

            yield return new WaitForEndOfFrame();
        }

        sourceCamera.transform.position = target.position;
        sourceCamera.transform.rotation = target.rotation;

        movementFinished?.Invoke();

        if (callback != null) callback();
    }
}
