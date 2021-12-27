using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class UserAchievementController : MonoBehaviour
{
    // Start is called before the first frame update
    async void Start()
    {
        UserController userController = UserController.GetInstance();
        if (userController == null) return;

        await UpdateUserAchievements(userController.user);
    }

    async Task UpdateUserAchievements(User user)
    {
        AchievementsController achievementsController = AchievementsController.GetInstance();
        if (achievementsController == null) return;

        List<Achievement> achievements = await DBUserAchievements.GetUserAchievements(user.id);

        achievements.ForEach(a => 
        {
            AchievementObject achievement = achievementsController.achievements.Find(achievement => a.id == achievement.id);
            if (achievement.title != a.title) achievement.title = a.title;
            if (achievement.description != a.description) achievement.title = a.title;
            if (achievement.received != a.received) achievement.received = a.received;
            if (achievement.receiveDate != a.receiveDate) achievement.receiveDate = a.receiveDate;
        });
    }
}
