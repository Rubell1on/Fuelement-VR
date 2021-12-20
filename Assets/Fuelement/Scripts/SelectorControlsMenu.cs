using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SelectorControlsMenu : MonoBehaviour
{
    public PagesController pages;
    public GameObject selectorCardTemplate;
    public Transform parent;
    public LevelCardHovered levelCardHovered = new LevelCardHovered();

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
        LevelsController levels = LevelsController.GetInstance();

        if (levels == null)
        {
            return;
        }

        for (int i = 0; i < levels.levels.Count; i++)
        {
            int id = i;
            CustomLevel level = levels.levels[id];

            GameObject instance = Instantiate(selectorCardTemplate, parent);
            LevelCard card = instance.GetComponent<LevelCard>();
            card.SetCardData(level);

            CustomButton cb = instance.GetComponent<CustomButton>();

            cb.onPointerEnter.AddListener(() =>
            {
                levelCardHovered?.Invoke(level);
            });

            cb.onClick.AddListener(() =>
            {
                levels.SelectLevelById(id);
                pages.OpenPage(1);
            });

            selectorCards.Add(instance);
        }
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

    //int GetCardId(GameObject card)
    //{
    //    return selectorCards.FindIndex(c => c.Equals(card));
    //}

    //CustomLevel GetLevel(GameObject card)
    //{
    //    int id = GetCardId(card);
    //    return selector.elements[id];
    //}

    [System.Serializable]
    public class LevelCardHovered : UnityEvent<CustomLevel> { }
}
