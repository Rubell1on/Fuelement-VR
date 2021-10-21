using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CycleController : MonoBehaviour
{
    public Drivetrain drivetrain;
    public Text text;
    public List<FloatMinMax> thresholds;

    public enum CycleType { CS, CF, H };
    // Start is called before the first frame update
    void Start()
    {
        if (drivetrain == null)
        {
            Debug.LogError("Отстутствует компонент Drivetrain!");
            this.enabled = false;
            return;
        }

        if (thresholds.Count < 3)
        {
            Debug.LogError("Массив thresholds должен содержать 3 элемента!");
            this.enabled = false;
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        if (text != null && drivetrain != null)
        {
            float veloKmh = drivetrain.velo * 3.6f;

            int i = thresholds.FindIndex(t => t.min < veloKmh && t.max > veloKmh ? true : false);

            text.text = $"{GetCycleString(i)}";
        }
    }

    //CycleType GetCycleById(int ind)
    //{
    //    int len = Enum.GetNames(typeof(CycleType)).Length;
    //    return ind < len ? (CycleType)ind : default(CycleType);
    //}

    string GetCycleString(int ind)
    {

        string[] cycles = Enum.GetNames(typeof(CycleType));
        return cycles[ind];
    }

    [Serializable]
    public class MinMax<T>
    {
        [SerializeField]
        public T min;
        [SerializeField]
        public T max;
    }

    [Serializable]
    public class FloatMinMax : MinMax<float> {
        public CycleType type;
    }
}