using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CarsController : Singleton<CarsController>
{
    public List<CarData> cars;
    public CarData currentSelection;

    public SelectorMenuEvent onSelectionChange;

    public void SelectCar(CarData newSelection)
    {
        if (newSelection != null)
        {
            currentSelection = newSelection;
            onSelectionChange.Invoke(currentSelection);
        }
    }

    public void DeselectCar()
    {
        currentSelection = null;
        onSelectionChange.Invoke(null);
    }

    [System.Serializable]
    public class SelectorMenuEvent : UnityEvent<CarData> { };
}
