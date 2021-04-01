using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelementHelper : MonoBehaviour
{
    public MatlabSocketAPI matlabSocketApi;
    public Text text;

    public float prevValue = 0;

    void Start()
    {
        if (!matlabSocketApi.connected)
        {
            gameObject.SetActive(false);
        }
    }

    public void OnMatlabResponseReceived(MatlabResponse res)
    {
        //Debug.Log($"Answer: {(res != null ? res.response : new MatlabResponse().response)}");
        float value = res != null ? res.response : prevValue;
        text.text = value.ToString();
        prevValue = value;
    }
}
