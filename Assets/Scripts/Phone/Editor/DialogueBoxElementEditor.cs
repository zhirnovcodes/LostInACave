using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DialogueBoxElement))]
public class DialogueBoxElementEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        DialogueBoxElement t = (DialogueBoxElement)target;

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Controls", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Set Scrolling Enabled"))
        {
            t.SetScrollingEnabled();
        }
        if (GUILayout.Button("Set Scrolling Disabled"))
        {
            t.SetScrollingDisabled();
        }
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Add Lost Test Message"))
        {
            t.AddMessage(new PhoneMessage { SenderType = SenderType.Lost, Message = "ttttttttttttttttttttttt" });
        }

        if (GUILayout.Button("Add Operator Test Message"))
        {
            t.AddMessage(new PhoneMessage { SenderType = SenderType.Operator, Message = "ttttttttttttttttttttttt" });
        }
    }
}
