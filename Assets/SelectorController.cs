using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SelectorController : MonoBehaviour
{
    public GameObject cardTemplate;
    public List<CustomLevel> elements;
    public CustomLevel currentSelection;

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

    public void setCardData(CustomLevel data)
    {
        if (data != null)
        {
            LevelCard card = cardInstance.GetComponent<LevelCard>();
            card.setCardData(data);
        }
    }

    public void SetSelection(CustomLevel newSelection)
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
    public class SelectorMenuEvent : UnityEvent<CustomLevel> { };
}
