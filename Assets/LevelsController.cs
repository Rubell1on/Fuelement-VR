using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelsController : Singleton<LevelsController>
{
    public List<CustomLevel> levels;
    public bool IsLast { get { return currentId < levels.Count - 1 ? true : false; } }
    public CustomLevel CurrentLevel { get { return levels[currentId]; } }
    public LevelSelectionChanged selectionChanged = new LevelSelectionChanged();
    [SerializeField]
    private int currentId;

    public CustomLevel SelectNext()
    {
        if (!IsLast)
        {
            currentId++;
            selectionChanged?.Invoke(currentId);
            return levels[currentId];
        }

        return null;
    }

    public CustomLevel SelectLevelById(int id)
    {
        if (id <= levels.Count - 1)
        {
            currentId = id;
            selectionChanged?.Invoke(currentId);
            return levels[currentId];
        }

        return null;
    }

    [System.Serializable]
    public class LevelSelectionChanged : UnityEvent<int> {}
}
