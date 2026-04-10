using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MessageBoxElement))]
public class MessageBoxElementEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MessageBoxElement t = (MessageBoxElement)target;

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Controls", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Set Typing Enabled"))
        {
            t.SetTypingEnabled();
        }
        if (GUILayout.Button("Set Typing Disabled"))
        {
            t.SetTypingDisabled();
        }
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Clear"))
        {
            t.Clear();
        }
    }
}
