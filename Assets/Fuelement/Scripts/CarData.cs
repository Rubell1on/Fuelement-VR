using UnityEngine;

[CreateAssetMenu(menuName = "Cars/CarData")]
public class CarData : ScriptableObject
{
    public string carFullName;
    public int releaseYear;
    public Texture producerLogo;
    public GameObject car;
    [HideInInspector]
    public Color32 background = new Color32(202, 202, 202, 255);
}