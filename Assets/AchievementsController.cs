using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class AchievementsController : Singleton<AchievementsController>
{
    [SerializeField]
    private AchievementUI template;
    [SerializeField]
    private RectTransform body;
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private List<Achievement> achievements = new List<Achievement>();
    public AchievementsControllerEvent achievementReceived = new AchievementsControllerEvent();

    public async void Receive(Achievement achievement)
    {
        if (achievement.received) return;
        if (await DBUserAchievements.AchievementReceived(2, achievement.id)) return;

        bool result = await DBUserAchievements.Receive(2, achievement.id);

        if (result)
        {
            achievement.received = true;

            AchievementUI achievementUI = CreateAchievementUI(achievement);
            achievementUI.ShowAndHide(() => DesroyAchievementUI(achievementUI));
            audioSource.Play();
        }
    }

    AchievementUI CreateAchievementUI(Achievement achievement)
    {
        AchievementUI achievementUI = Instantiate(template, body.transform);
        achievementUI.Title = achievement.title;
        achievementUI.Description = achievement.description;

        return achievementUI;
    }

    void DesroyAchievementUI(AchievementUI achievementUI)
    {
        Destroy(achievementUI.gameObject);
    }

    [System.Serializable]
    public class AchievementsControllerEvent : UnityEvent<Achievement> { }
}