using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

[CustomEditor(typeof(CustomButton))]
public class CustomButtonEditor : UnityEditor.UI.ButtonEditor
{
    public override void OnInspectorGUI()
    {
        CustomButton component = (CustomButton)target;

        base.OnInspectorGUI();
        SerializedProperty onPointerEnter = serializedObject.FindProperty("onPointerEnter");
        //EditorGUIUtility.LookLikeControls();
        EditorGUILayout.PropertyField(onPointerEnter);

        if (GUI.changed)
        {
            serializedObject.ApplyModifiedProperties();
        }
    }
}
