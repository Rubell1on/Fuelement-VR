using UnityEngine;

[CreateAssetMenu(fileName = "Achievement", menuName = "Achievement")]
public class Achievement : ScriptableObject
{
    public int id;
    public string title;
    public string description;
    public bool received = false;
}