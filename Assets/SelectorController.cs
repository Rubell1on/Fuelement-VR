﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorController : MonoBehaviour
{
    public GameObject cardTemplate;
    public List<CustomLevel> elements;

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
        LevelCard card = cardInstance.GetComponent<LevelCard>();
        card.setCardData(data);
    }
}
