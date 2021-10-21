using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CarSelectorControlsMenu : MonoBehaviour
{
    public GameObject selectorCardTemplate;
    public Transform parent;

    List<GameObject> selectorCards = new List<GameObject>();

    CarsController carsController;

    private void OnEnable()
    {
        carsController = CarsController.GetInstance();
        InitCards();
    }

    private void OnDisable()
    {
        RemoveAllCards();
    }

    void InitCards()
    {
        carsController.cars.ForEach(l =>
        {
            GameObject instance = Instantiate(selectorCardTemplate, parent);
            CarSelectorCard card = instance.GetComponent<CarSelectorCard>();
            card.setCardData(l);
            CustomButton cb = instance.GetComponent<CustomButton>();

            cb.onPointerEnter.AddListener(() => {
                CarData cl = GetCar(instance);
                carsController.onSelectionChange.Invoke(cl);
            });

            cb.onClick.AddListener(() =>
            {
                carsController.SelectCar(l);
            });
            selectorCards.Add(instance);
        });
    }

    void RemoveAllCards()
    {
        selectorCards.ForEach(l => {
            Destroy(l);
            CustomButton cb = l.GetComponent<CustomButton>();
            cb.onClick.RemoveAllListeners();
        });

        selectorCards = new List<GameObject>();
    }

    int GetCardId(GameObject card)
    {
        return selectorCards.FindIndex(c => c.Equals(card));
    }

    CarData GetCar(GameObject card)
    {
        int id = GetCardId(card);
        return carsController.cars[id];
    }
}