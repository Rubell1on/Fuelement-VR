using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DynamicCameraController : MonoBehaviour
{
    public Transform target;
    [Header("Main settings")]
    public Camera sourceCamera;
    public Transform[] cameraPlaces;
    public int currentPlaceId = 0;
    public float speedAmplifier = 1;
    public bool cycled = false;
    public float lerpThreshold = 0.99f;

    [Space(10f)]
    [Header("Animation settings")]
    public bool useCurve = true;
    public AnimationCurve animationCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0, 0), new Keyframe(1, 1) });
    [Space(10f)]
    [Header("Events")]
    public MovementFinished movementFinished;

    Coroutine currentCoroutine;

    private void Start()
    {
        if (target != null)
        {
            transform.parent = target;
        }
    }

    [ContextMenu("Next")]
    public void Next()
    {
        if (sourceCamera == null) return;
        if (cameraPlaces.Length == 0) return;

        if (currentPlaceId + 1 <= cameraPlaces?.Length - 1)
        {
            currentPlaceId++;
        } 
        else if (cycled == true && currentPlaceId + 1 > cameraPlaces?.Length - 1)
        {
            currentPlaceId = 0;
        }

        MoveTo(currentPlaceId);
    }

    [ContextMenu("Next instant")]
    public void NextInstant()
    {
        if (sourceCamera == null) return;
        if (cameraPlaces.Length == 0) return;

        if (currentPlaceId + 1 <= cameraPlaces?.Length - 1)
        {
            currentPlaceId++;
        }
        else if (cycled == true && currentPlaceId + 1 > cameraPlaces?.Length - 1)
        {
            currentPlaceId = 0;
        }

        Transform target = cameraPlaces[currentPlaceId];
        sourceCamera.transform.position = target.transform.position;
        sourceCamera.transform.rotation = target.transform.rotation;

        movementFinished?.Invoke(currentPlaceId);
    }

    public void MoveTo(int id = 0)
    {
        if (sourceCamera == null) return;
        if (cameraPlaces.Length == 0) return;

        if (id <= cameraPlaces?.Length - 1)
        {
            currentPlaceId = id;
            Transform target = cameraPlaces[id];
            if (currentCoroutine != null) StopCoroutine(currentCoroutine);
            currentCoroutine = StartCoroutine(MoveTo(target));
        }
    }

    public void MoveToCurrent()
    {
        MoveTo(currentPlaceId);
    }

    public void MoveToInstant(int id = 0)
    {
        if (sourceCamera != null && id <= cameraPlaces?.Length - 1)
        {
            currentPlaceId = id;
            Transform target = cameraPlaces[id];
            sourceCamera.transform.position = target.transform.position;
            sourceCamera.transform.rotation = target.transform.rotation;

            movementFinished?.Invoke(currentPlaceId);
        }
    }

    public void MoveToCurrentInstant()
    {
        MoveToInstant(currentPlaceId);
    }

    public void StopAnimation()
    {
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);
    }

    IEnumerator MoveTo(Transform target, Action callback = null)
    {
        Vector3 currentPosition = sourceCamera.transform.position;
        Quaternion currentRotation = sourceCamera.transform.rotation;

        float lerpTime = 0;

        while (lerpTime < lerpThreshold)
        {
            float value = lerpTime;
            value = animationCurve.Evaluate(lerpTime);
            currentPosition = Vector3.Lerp(currentPosition, target.position, value);
            currentRotation = Quaternion.Lerp(currentRotation, target.rotation, value);

            sourceCamera.transform.position = currentPosition;
            sourceCamera.transform.rotation = currentRotation;

            lerpTime += Time.deltaTime * speedAmplifier;

            if (!currentPosition.Equals(target.position) || !currentRotation.Equals(target.rotation))
                yield return new WaitForEndOfFrame();
        }

        sourceCamera.transform.position = target.position;
        sourceCamera.transform.rotation = target.rotation;

        movementFinished?.Invoke(currentPlaceId);

        if (callback != null) callback();
    }

    [Serializable]
    public class MovementFinished : UnityEvent<int> { }
}
