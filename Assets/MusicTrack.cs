﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Music/MusicTrack")]
public class MusicTrack : ScriptableObject
{
    public List<string> artists;
    public string songName;
    public string albumName;
    public int year;
    public AudioClip audioClip;
}
