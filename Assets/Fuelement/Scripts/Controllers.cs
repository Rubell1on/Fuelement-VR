using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Controllers : MonoBehaviour
{
    Dictionary<string, float> values1 = new Dictionary<string, float>()
    {
        { "Keyboard_Horizontal", 0f },
        { "Keyboard_Throttle", 0f },
        { "Keyboard_Break", 0f },
        { "Keyboard_Clutch", 0f },
        { "Keyboard_StartEngine", 0f },
        { "Keyboard_Handbreak", 0f },
        { "Keyboard_ShiftUp", 0f },
        { "Keyboard_ShiftDown", 0f }
    };

    // Start is called before the first frame update
    public string buttonName = "Joystick1Button3";
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        System.Array values = System.Enum.GetValues(typeof(KeyCode));
        foreach (KeyCode code in values)
        {
            if (Input.GetKeyDown(code)) { print(System.Enum.GetName(typeof(KeyCode), code)); }
        }

        //print(Input.GetAxis("Horizontal"));

        if (Input.GetKeyDown(KeyCode.Joystick1Button3))
        {
            print("hello");
        }

        float k_hor = Input.GetAxis("Keyboard_Horizontal");

        string[] tempStr = values1.Keys
            .ToList()
            .Select(k =>
            {
                float v = Input.GetAxis(k);
                return $"{k}: {v}";
            })
            .ToArray();

        print(string.Join(" ", tempStr));
    }
}
