using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CarSelectorController : MonoBehaviour
{
    public GameObject cardTemplate;
    public GameModeController gameModeController;

    GameObject cardInstance;
    CarsController carsController;

    void OnEnable()
    {
        carsController = CarsController.GetInstance();
        carsController.onSelectionChange.AddListener(SetCardData);
        carsController.onSelectionChange.AddListener(gameModeController.ChangeState);

        if (carsController.cars.Count > 0)
        {
            cardInstance = Instantiate(cardTemplate, gameObject.transform);
            cardInstance.transform.SetAsFirstSibling();
            SetCardData(carsController.cars[0]);
        }
        else
        {
            Debug.LogError("Список элементов пуст!");
        }
    }

    void OnDisable()
    {
        carsController.onSelectionChange.RemoveListener(SetCardData);
        carsController.onSelectionChange.RemoveListener(gameModeController.ChangeState);

        Destroy(cardInstance);
    }

    public void SetCardData(CarData data)
    {
        if (data != null)
        {
            CarCard card = cardInstance.GetComponent<CarCard>();
            card.setCardData(data);
        }
    }
}