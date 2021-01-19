using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelSelectorMenu : MonoBehaviour
{
    public LevelsSelector selector;
    public GameObject levelSelectorCardTemplate;
    public Transform parent;
    public LevelSelectorMenuEvent onSelectionChande;

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
        selector.levels.ForEach(l =>
        {
            GameObject instance = Instantiate(levelSelectorCardTemplate, parent);
            LevelCard card = instance.GetComponent<LevelCard>();
            card.setLevelInfo(l);
            CustomButton cb = instance.GetComponent<CustomButton>();
            cb.onClick.AddListener(() => {
                CustomLevel cl = GetLevel(instance);
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

    CustomLevel GetLevel(GameObject card)
    {
        int id = GetCardId(card);
        return selector.levels[id];
    }

    [System.Serializable]
    public class LevelSelectorMenuEvent : UnityEvent<CustomLevel> { };
}
