using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{
    public TriggerEvent EnteredTrigger = new TriggerEvent();

    private void OnTriggerEnter(Collider other)
    {
        Drivetrain drivetrain = other.gameObject.GetComponentInParent<Drivetrain>();
        if (drivetrain != null)
        {
            EnteredTrigger.Invoke(other);
        }
    }

    [System.Serializable]
    public class TriggerEvent : UnityEvent<Collider> { }
}
