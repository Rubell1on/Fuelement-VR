using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Levels/CustomLevel")]

[Serializable]
public class CustomLevel : ASimpleData
{
    [SerializeField]
    public int sceneBuiltInId;
}
