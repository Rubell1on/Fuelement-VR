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
        SerializedProperty source = serializedObject.FindProperty("source");
        EditorGUILayout.PropertyField(source);
        SerializedProperty pointerEnter = serializedObject.FindProperty("pointerEnter");
        EditorGUILayout.PropertyField(pointerEnter);
        SerializedProperty pointerExit = serializedObject.FindProperty("pointerExit");
        EditorGUILayout.PropertyField(pointerExit);
        SerializedProperty pointerClick = serializedObject.FindProperty("pointerClick");
        EditorGUILayout.PropertyField(pointerClick);
        SerializedProperty onPointerEnter = serializedObject.FindProperty("onPointerEnter");
        EditorGUILayout.PropertyField(onPointerEnter);
        SerializedProperty onPointerExit = serializedObject.FindProperty("onPointerExit");
        EditorGUILayout.PropertyField(onPointerExit);

        if (GUI.changed)
        {
            serializedObject.ApplyModifiedProperties();
        }
    }
}
