using System.Collections.Generic;
using UnityEngine;

public class GridSnapper : MonoBehaviour
{
    public Grid Grid;
    public List<Transform> Transforms;

    public void snapToGrid()
    {
        if (Grid == null)
        {
            Debug.LogWarning("[GridSnapper] Grid is not assigned.");
            return;
        }

        foreach (Transform T in Transforms)
        {
            if (T == null)
            {
                continue;
            }

            Vector3Int Cell = Grid.WorldToCell(T.position);
            Vector3 CellCenter = Grid.GetCellCenterWorld(Cell);
            T.position = CellCenter;
        }
    }
}
