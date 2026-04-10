using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MockNetModel))]
public class MockNetModelEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MockNetModel mock = (MockNetModel)target;

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Mock Controls", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Receive 1 Message"))
        {
            mock.QueueRandomMessage();
        }
        if (GUILayout.Button("Receive 2 Messages"))
        {
            mock.QueueRandomMessage();
            mock.QueueRandomMessage();
        }
        EditorGUILayout.EndHorizontal();
    }
}
