using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SemaphoreController : MonoBehaviour
{
    public bool enableOnStart = false;
    public enum MovementDirection { forward, side };
    public float duration = 5;
    public MovementDirection movementDirection = MovementDirection.forward;
    public List<Semaphore> forward = new List<Semaphore>();
    public List<Semaphore> side = new List<Semaphore>();

    private Coroutine sectionStateSetting;

    [ContextMenu("Enable")]
    public void Enable()
    {
        if (movementDirection == MovementDirection.forward)
        {
            sectionStateSetting = StartCoroutine(SetSectionsState(forward, side));
        }
        else
        {
            sectionStateSetting = StartCoroutine(SetSectionsState(side, forward));
        }
    }

    [ContextMenu("Disable")]
    public void Disable()
    {
        StopCoroutine(sectionStateSetting);
    }

    IEnumerator SetSectionsState(List<Semaphore> oneDirection, List<Semaphore> anotherDirection)
    {
        Semaphore green = oneDirection[0];
        Semaphore red = anotherDirection[0];

        anotherDirection.GetRange(1, anotherDirection.Count - 1).ForEach(s => StartCoroutine(s.SetState(SemaphoreSection.SectionType.red)));

        yield return StartCoroutine(red.SetState(SemaphoreSection.SectionType.red));

        oneDirection.GetRange(1, oneDirection.Count - 1).ForEach(s => StartCoroutine(s.SetState(SemaphoreSection.SectionType.green)));
        
        yield return StartCoroutine(green.SetState(SemaphoreSection.SectionType.green));
        
        yield return new WaitForSeconds(duration);

        movementDirection = movementDirection == MovementDirection.forward
                ? MovementDirection.side
                : MovementDirection.forward;

        Enable();
    }

    void Start()
    {
        if (enableOnStart)
        {
            List<Semaphore> semaphores = new List<Semaphore>(forward);
            semaphores.AddRange(side);

            semaphores.ForEach(s => {
                s.enableOnStart = false;

                if (s.SettingState)
                {
                    s.StopSetState();
                }
            });

            Enable();
        }
    }
}
