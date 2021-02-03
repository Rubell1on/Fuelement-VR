using System;
using UnityEngine;
using UnityEngine.Events;

public class AdvicesController : MonoBehaviour
{
    public string[] advices;
    public OnChangeEvent onChange;
    int currIndex;

    private void Start()
    {
        GetRandomAdvice();
    }

    public void GetRandomAdvice()
    {
        if (advices.Length > 0)
        {
            currIndex = UnityEngine.Random.Range(0, advices.Length);
            onChange.Invoke(advices[currIndex]);
        }
        else
        {
            throw new Exception("Advices array is empty!");
        }
    }

    public void Next()
    {
        if (advices.Length > 0)
        {
            if (advices.Length > currIndex + 1)
            {
                currIndex += 1;
            } else
            {
                currIndex = 0;
            }

            onChange.Invoke(advices[currIndex]);
        }
        else
        {
            throw new Exception("Advices array is empty!");
        }
    }

    public void Previous()
    {
        if (advices.Length > 0)
        {
            if (currIndex - 1 >= 0)
            {
                currIndex -= 1;
            }
            else
            {
                currIndex = advices.Length - 1;
            }

            onChange.Invoke(advices[currIndex]);
        }
        else
        {
            throw new Exception("Advices array is empty!");
        }
    }

    [Serializable]
    public class OnChangeEvent : UnityEvent<string> { };
}
