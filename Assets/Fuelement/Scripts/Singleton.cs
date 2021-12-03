using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    bool doNotDestroyOnLoad = true;
    private static T _instance;

    public void Awake()
    {
        if (_instance == null)
        {
            _instance = GetComponent<T>();

            if (doNotDestroyOnLoad) DontDestroyOnLoad(this);

            return;
        }

        Destroy(gameObject);
    }

    public static T GetInstance()
    {
        return _instance;
    } 

    public void SetDoNotDestroyOnLoad(bool value)
    {
        this.doNotDestroyOnLoad = value;
    }
}
