using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsSelector : MonoBehaviour
{
    public GameObject cardTemplate;
    public List<CustomLevel> levels;

    GameObject cardInstance;

    void OnEnable()
    {
        if (levels.Count > 0)
        {
            cardInstance = Instantiate(cardTemplate, gameObject.transform);
            cardInstance.transform.SetAsFirstSibling();
            setCardInfo(levels[0]);
        } else
        {
            Debug.LogError("Список уровней пуст!");
        }
        
    }

    void OnDisable()
    {
        Destroy(cardInstance);
    }

    public void setCardInfo(CustomLevel level)
    {
        LevelCard card = cardInstance.GetComponent<LevelCard>();
        card.setLevelInfo(level);
    }
}
