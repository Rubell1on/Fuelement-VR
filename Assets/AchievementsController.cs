using System;
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
    public List<AchievementObject> achievements = new List<AchievementObject>();
    public AchievementsControllerEvent achievementReceived = new AchievementsControllerEvent();

    public async void Receive(AchievementObject achievement)
    {
        if (achievement.received) return;
        if (await DBUserAchievements.AchievementReceived(2, achievement.id)) return;

        DateTime receiveDate = DateTime.Now;
        bool result = await DBUserAchievements.Receive(2, achievement.id, receiveDate);

        if (result)
        {
            achievement.received = true;
            achievement.receiveDate = receiveDate;

            AchievementUI achievementUI = CreateAchievementUI(achievement);
            achievementUI.ShowAndHide(() => DesroyAchievementUI(achievementUI));
            audioSource.Play();
        }
    }

    public void ReceiveDemo(AchievementObject achievement)
    {
        AchievementUI achievementUI = CreateAchievementUI(achievement);
        achievementUI.ShowAndHide(() => DesroyAchievementUI(achievementUI));
        audioSource.Play();

    }

    public AchievementUI CreateAchievementUI(AchievementObject achievement)
    {
        AchievementUI achievementUI = Instantiate(template, body.transform);
        achievementUI.Title = achievement.title;
        achievementUI.Description = achievement.description;

        return achievementUI;
    }

    public void DesroyAchievementUI(AchievementUI achievementUI)
    {
        Destroy(achievementUI.gameObject);
    }

    [System.Serializable]
    public class AchievementsControllerEvent : UnityEvent<AchievementObject> { }
}