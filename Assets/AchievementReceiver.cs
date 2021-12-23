using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementReceiver : MonoBehaviour
{
    public Achievement achievement;

    [ContextMenu("Receive")]
    public void Receive()
    {
        AchievementsController achievementsController = AchievementsController.GetInstance();
        if (achievement == null || achievementsController == null) return;

        achievementsController.Receive(achievement);
    }
}
