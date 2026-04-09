using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GridSnapper))]
public class GridSnapperEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GUILayout.Space(8);

        GridSnapper Snapper = (GridSnapper)target;

        if (GUILayout.Button("Snap Transforms to Cell Centers"))
        {
            if (Snapper.Transforms != null)
            {
                foreach (Transform T in Snapper.Transforms)
                {
                    if (T != null)
                    {
                        Undo.RecordObject(T, "Snap to Grid Cell Centers");
                    }
                }
            }

            Snapper.snapToGrid();
        }
    }
}
