using System.Collections.Generic;
using UnityEngine;

public class CollidersCulling : MonoBehaviour
{
    [Header("References")]
    public Collider[] Colliders;
    public Transform Player;
    public Grid Grid;

    [Header("Visible Range")]
    public Vector2Int VisibleSize = new Vector2Int(5, 5);

    private Dictionary<Vector3Int, List<Collider>> CellMap;
    private HashSet<Vector3Int> ActiveCells = new HashSet<Vector3Int>();
    private Vector3Int LastPlayerCell;

    private void Awake()
    {
        BuildCellMap();
    }

    private void OnEnable()
    {
        if (CellMap == null)
        {
            BuildCellMap();
        }

        DisableAll();
        ActiveCells.Clear();
        LastPlayerCell = GetPlayerCell();
        EnableCellsInRange(LastPlayerCell);
    }

    private void Update()
    {
        Vector3Int currentCell = GetPlayerCell();

        if (currentCell == LastPlayerCell)
        {
            return;
        }

        DisableCells(ActiveCells);
        ActiveCells.Clear();
        EnableCellsInRange(currentCell);
        LastPlayerCell = currentCell;
    }

    private void BuildCellMap()
    {
        CellMap = new Dictionary<Vector3Int, List<Collider>>();

        foreach (Collider collider in Colliders)
        {
            Vector3Int rawCell = Grid.WorldToCell(collider.transform.position);
            Vector3Int cellPosition = new Vector3Int(rawCell.x, 0, rawCell.z);

            if (!CellMap.ContainsKey(cellPosition))
            {
                CellMap[cellPosition] = new List<Collider>(4);
            }

            CellMap[cellPosition].Add(collider);
        }
    }

    private void DisableAll()
    {
        foreach (Collider collider in Colliders)
        {
            collider.enabled = false;
        }
    }

    private void DisableCells(HashSet<Vector3Int> targetCells)
    {
        foreach (Vector3Int cellPosition in targetCells)
        {
            if (!CellMap.TryGetValue(cellPosition, out List<Collider> cellColliders))
            {
                continue;
            }

            foreach (Collider collider in cellColliders)
            {
                collider.enabled = false;
            }
        }
    }

    private void EnableCellsInRange(Vector3Int centerCell)
    {
        int halfRangeX = VisibleSize.x / 2;
        int halfRangeZ = VisibleSize.y / 2;

        for (int xOffset = -halfRangeX; xOffset <= halfRangeX; xOffset++)
        {
            for (int zOffset = -halfRangeZ; zOffset <= halfRangeZ; zOffset++)
            {
                Vector3Int cellPosition = new Vector3Int(centerCell.x + xOffset, centerCell.y, centerCell.z + zOffset);
                ActiveCells.Add(cellPosition);

                if (!CellMap.TryGetValue(cellPosition, out List<Collider> cellColliders))
                {
                    continue;
                }

                foreach (Collider collider in cellColliders)
                {
                    collider.enabled = true;
                }
            }
        }
    }

    private Vector3Int GetPlayerCell()
    {
        Vector3Int cellPosition = Grid.WorldToCell(Player.position);
        return new Vector3Int(cellPosition.x, 0, cellPosition.z);
    }
}
