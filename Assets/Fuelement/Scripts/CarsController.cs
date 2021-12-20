using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CarsController : Singleton<CarsController>
{
    public List<CarData> cars;
    public CarData CurrentCar { get { return currentCar; } }
    private CarData currentCar;

    public SelectorMenuEvent onSelectionChange;

    public void SelectCar(CarData newSelection)
    {
        if (newSelection != null)
        {
            currentCar = newSelection;
            onSelectionChange.Invoke(currentCar);
        }
    }

    public void DeselectCar()
    {
        currentCar = null;
        onSelectionChange.Invoke(null);
    }

    [System.Serializable]
    public class SelectorMenuEvent : UnityEvent<CarData> { };
}
