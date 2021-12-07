using UnityEngine;

public class Rotatator : MonoBehaviour
{
    public GameObject source;
    public Vector3 targetAngle = new Vector3(0f, 0f, 30f);
    public float speedAmplifier = 1;
    public string axisName;
    public bool checkingAxis = false;
    public AnimationCurve animationCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0, 0), new Keyframe(1,1) });

    Vector3 initialRotation;

    void Start()
    {
        if (source != null)
        {
            initialRotation = source.transform.localRotation.eulerAngles;
        }
    }

    public void Rotate(float value, Vector3 targetAngle)
    {
        if (source == null) return;

        if (value >= 0)
        {
            float time = animationCurve.Evaluate(value);
            Vector3 lerpedAngle = Vector3.Lerp(initialRotation, targetAngle, time * speedAmplifier);
            source.transform.localRotation = Quaternion.Euler(lerpedAngle);
        }
    }

    public void Update()
    {
        if (checkingAxis)
        {
            float value = InputExtension.GetAxis(axisName);
            Debug.Log(value.ToString());
            Rotate(value, targetAngle);
        }
    }
}