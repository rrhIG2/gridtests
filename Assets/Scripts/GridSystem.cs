using UnityEngine;
using System.Collections.Generic;

public class GridSystem : MonoBehaviour
{
    [Header("Grid Settings")]
    public float cellSize = 1f;
    public int viewRadius = 5;
    public Transform cameraTransform;
    public GameObject gridCellPrefab;
    public Transform baseTransform;
    public Transform gridParent;

    private Dictionary<Vector2Int, GameObject> activeCells = new Dictionary<Vector2Int, GameObject>();

    void Start()
    {
        // If gridParent is not assigned, create it dynamically
        if (gridParent == null)
        {
            GameObject gridParentObject = new GameObject("GridCells");
            gridParent = gridParentObject.transform;
        }
    }

    void Update()
    {
        UpdateVisibleCells();
    }

    void UpdateVisibleCells()
    {
        Vector2Int cameraGridPosition = GetGridPosition(cameraTransform.position);
        Vector2Int baseGridPosition = GetGridPosition(baseTransform.position);

        for (int x = -viewRadius; x <= viewRadius; x++)
        {
            for (int y = -viewRadius; y <= viewRadius; y++)
            {
                Vector2Int gridPos = new Vector2Int(cameraGridPosition.x + x, cameraGridPosition.y + y);

                if (!activeCells.ContainsKey(gridPos))
                {
                    CreateGridCell(gridPos, baseGridPosition);
                }
            }
        }

        // Remove cells outside of the radius
        List<Vector2Int> cellsToRemove = new List<Vector2Int>();

        foreach (var cell in activeCells)
        {
            if (Vector2Int.Distance(cell.Key, cameraGridPosition) > viewRadius)
            {
                Destroy(activeCells[cell.Key]);
                cellsToRemove.Add(cell.Key);
            }
        }

        foreach (var pos in cellsToRemove)
        {
            activeCells.Remove(pos);
        }
    }

    Vector2Int GetGridPosition(Vector3 worldPosition)
    {
        int x = Mathf.FloorToInt(worldPosition.x / cellSize);
        int y = Mathf.FloorToInt(worldPosition.z / cellSize);
        return new Vector2Int(x, y);
    }

    void CreateGridCell(Vector2Int gridPos, Vector2Int baseGridPosition)
    {
        Vector3 worldPosition = new Vector3(gridPos.x * cellSize, 0, gridPos.y * cellSize);
        GameObject gridCell = Instantiate(gridCellPrefab, worldPosition, Quaternion.identity, gridParent);
        gridCell.name = $"GridCell_{gridPos.x}_{gridPos.y}";

        int localX = gridPos.x - baseGridPosition.x;
        int localY = gridPos.y - baseGridPosition.y;

        GridCell cellScript = gridCell.GetComponent<GridCell>();
        cellScript.Initialize(localX, localY);

        activeCells.Add(gridPos, gridCell);
    }
}

