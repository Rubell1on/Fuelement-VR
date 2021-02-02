using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSelectorController : MonoBehaviour
{
    public GameObject cardTemplate;
    public List<CarData> elements;

    GameObject cardInstance;

    void OnEnable()
    {
        if (elements.Count > 0)
        {
            cardInstance = Instantiate(cardTemplate, gameObject.transform);
            cardInstance.transform.SetAsFirstSibling();
            setCardData(elements[0]);
        }
        else
        {
            Debug.LogError("Список элементов пуст!");
        }
    }

    void OnDisable()
    {
        Destroy(cardInstance);
    }

    public void setCardData(CarData data)
    {
        CarCard card = cardInstance.GetComponent<CarCard>();
        card.setCardData(data);
    }
}
