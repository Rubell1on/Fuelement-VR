using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SelectorControlsMenu : MonoBehaviour
{
    public SelectorController selector;
    public PagesController pages;
    public GameObject selectorCardTemplate;
    public Transform parent;

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
            LevelCard card = instance.GetComponent<LevelCard>();
            card.setCardData(l);
            CustomButton cb = instance.GetComponent<CustomButton>();
            
            cb.onPointerEnter.AddListener(() =>
            {
                CustomLevel cl = GetLevel(instance);
                selector.onSelectionChange.Invoke(cl);
            });
            cb.onClick.AddListener(() =>
            {
                selector.SetSelection(l);
                pages.OpenPage(1);
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

    CustomLevel GetLevel(GameObject card)
    {
        int id = GetCardId(card);
        return selector.elements[id];
    }
}
