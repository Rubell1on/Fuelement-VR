using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Achievement", menuName = "Achievement")]
public class AchievementObject : ScriptableObject
{
    public int id;
    public string title;
    public string description;
    public bool received = false;
    public DateTime receiveDate;
}

public class Achievement
{
    public int id;
    public string title;
    public string description;
    public bool received = false;
    public DateTime receiveDate;
}