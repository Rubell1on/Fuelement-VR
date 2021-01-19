using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Levels/CustomLevel")]
public class CustomLevel : ScriptableObject
{
    public Scene id;
    public string title;
    public string description;
    public Texture background;
}
