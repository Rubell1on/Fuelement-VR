using UnityEngine;

public class Rotator : MonoBehaviour
{
    public GameObject source;
    public Vector3 targetAngle = new Vector3(0f, 0f, 30f);
    public float speedAmplifier = 1;

    [Space(10f)]
    [Header("Axis settings")]
    public bool checkingAxis = false;
    public string axisName;

    [Space(10f)]
    [Header("Animation settings")]
    public bool useCurve = true;
    public AnimationCurve animationCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0, 0), new Keyframe(1,1) });

    [Space(10f)]
    [Header("Audio settings")]
    public AudioSource audioSource;
    public AudioClip handbrakeUp;
    public AudioClip handbrakeDown;

    Vector3 initialRotation;

    float previousValue = 0;
    bool raise = false;
    int brakePhase = 0;

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
            if (useCurve) value = animationCurve.Evaluate(value);
            Vector3 lerpedAngle = Vector3.Lerp(initialRotation, targetAngle, value * speedAmplifier);
            source.transform.localRotation = Quaternion.Euler(lerpedAngle);
        }
    }

    public void Update()
    {
        if (checkingAxis)
        {
            float value = InputExtension.GetAxis(axisName);

            raise = value > previousValue ? true : false;
            Rotate(value, targetAngle);

            if (raise)
            {
                int div = (int)(value / 0.33f);
                if (div != brakePhase)
                {
                    brakePhase = div;

                    if (audioSource != null && handbrakeUp != null)
                    {
                        audioSource.clip = handbrakeUp;
                        audioSource.Play();
                    }
                }
            }
            else if (!raise && value < 0.33f && brakePhase != 0)
            {
                brakePhase = 0;
                if (audioSource != null && handbrakeDown != null)
                {
                    audioSource.clip = handbrakeDown;
                    if (!audioSource.isPlaying)
                    {
                        audioSource.Play();
                    }
                }
            }

            previousValue = value;
        }
    }
}