using UnityEngine;

[CreateAssetMenu(fileName = "Achievement", menuName = "Achievement")]
public class Achievement : ScriptableObject
{
    public int id;
    public string title;
    public string description;
    public bool received = false;

    [ContextMenu("Receive")]
    public void Receive()
    {
        if (received) return;

        AchievementsController achievementController = AchievementsController.GetInstance();

        if (achievementController == null) return;

        achievementController.Receive(this);
        received = true;
    }

}