using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CarSelectorController : MonoBehaviour
{
    public GameObject cardTemplate;
    public List<CarData> elements;
    public CarData currentSelection;

    public SelectorMenuEvent onSelectionChange;

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
        if (data != null)
        {
            CarCard card = cardInstance.GetComponent<CarCard>();
            card.setCardData(data);
        }
    }

    public void SetSelection(CarData newSelection)
    {
        if (newSelection != null)
        {
            currentSelection = newSelection;
            onSelectionChange.Invoke(currentSelection);
        }
    }

    public void RemoveSelection()
    {
        currentSelection = null;
        onSelectionChange.Invoke(null);
    }

    [System.Serializable]
    public class SelectorMenuEvent : UnityEvent<CarData> { };
}
