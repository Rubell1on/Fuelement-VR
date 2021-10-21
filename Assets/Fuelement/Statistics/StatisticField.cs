using UnityEngine;
using UnityEngine.UI;

public class StatisticField : MonoBehaviour
{
    public Text _label;
    public Text _value;

    public float threshold = 0;

    public string Label { 
        get { return _label.text; } 
        set { _label.text = value; } 
    }

    public float Value
    {
        get
        {
            float result = 0;

            if (float.TryParse(_value.text, out result))
            {
                Debug.LogError("Произошла ошибка при попытке распарсить значение!");
            }

            return result;
        }

        set
        {
            _value.text = value.ToString();
            _value.color = value >= threshold ? Color.red : Color.green;
        }
    }
}
