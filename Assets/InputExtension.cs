using UnityEngine;

public static class InputExtension
{
    public static float GetAxis(string axisName)
    {
        float value = 0;

        float axisValue = Input.GetAxis(axisName);
        float kbAxisValue = Input.GetAxis($"Keyboard_{axisName}");

        if (axisValue >= 0 || kbAxisValue >= 0)
        {
            value = axisValue >= 0 ? axisValue : kbAxisValue;
        }

        return value;
    }
}