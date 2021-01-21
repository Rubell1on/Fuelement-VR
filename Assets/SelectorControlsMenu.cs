using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SelectorControlsMenu : MonoBehaviour
{
    public SelectorController selector;
    public GameObject selectorCardTemplate;
    public Transform parent;
    public SelectorMenuEvent onSelectionChande;

    List<GameObject> selectorCards = new List<GameObject>();

    private void OnEnable()
    {
        InitCards();
    }

    private void OnDisable()
    {
        RemoveAllCards();
    }

    void InitCards()
    {
        selector.elements.ForEach(l =>
        {
            GameObject instance = Instantiate(selectorCardTemplate, parent);
            ISimpleDataCard card = instance.GetComponent<ISimpleDataCard>();
            card.setCardData(l);
            CustomButton cb = instance.GetComponent<CustomButton>();
            cb.onClick.AddListener(() => {
                ASimpleData cl = GetLevel(instance);
                onSelectionChande.Invoke(cl);
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

    ASimpleData GetLevel(GameObject card)
    {
        int id = GetCardId(card);
        return selector.elements[id];
    }

    [System.Serializable]
    public class SelectorMenuEvent : UnityEvent<ASimpleData> { };
}
