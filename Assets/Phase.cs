using UnityEngine;
using UnityEngine.Events;

public class Phase : MonoBehaviour
{
    public UnityEvent started = new UnityEvent();
    public UnityEvent finished = new UnityEvent();
    public UnityEvent failed = new UnityEvent();

    public virtual void StartPhase() 
    {
        started?.Invoke();
    }

    [ContextMenu("Restart phase")]
    public virtual void RestartPhase()
    {
        ResetValues();
        StartPhase();
    }

    public virtual void ResetValues() {}
}
