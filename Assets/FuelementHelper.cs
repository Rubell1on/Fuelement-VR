using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelementHelper : MonoBehaviour
{
    public void OnMatlabResponseReceived(MatlabResponse res)
    {
        Debug.Log($"Answer: {(res != null ? res.response : new MatlabResponse().response)}");
    }
}
