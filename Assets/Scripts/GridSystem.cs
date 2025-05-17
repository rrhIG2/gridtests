using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;

public class GridSystem : MonoBehaviour
{
    [Header("Grid Settings")]
    public float cellSize = 1f;
    public int viewRadius = 5;
    public Transform cameraTransform;
    public GameObject gridCellPrefab;
    public Transform baseTransform;
    public Transform gridParent;

    [Header("References")]
    public DatabaseManager databaseManager;

    [SerializeField] private Dictionary<Vector2Int, GameObject> activeCells = new Dictionary<Vector2Int, GameObject>();
    [SerializeField] private Dictionary<Vector2Int, GridData> cachedData = new Dictionary<Vector2Int, GridData>();
    private Vector2Int lastCameraPosition;

    void Start()
    {
        if (gridParent == null)
        {
            GameObject gridParentObject = new GameObject("GridCells");
            gridParent = gridParentObject.transform;
        }

        lastCameraPosition = GetGridPosition(cameraTransform.position);

        // Create visible cells immediately
        UpdateVisibleCells();

        // After 10 seconds, fetch the initial data
        StartCoroutine(FetchAndCacheData());
    }

    void Update()
    {
        Vector2Int currentPos = GetGridPosition(cameraTransform.position);

        if (currentPos != lastCameraPosition)
        {
            UpdateVisibleCells();
            lastCameraPosition = currentPos;

            // Fetch new data if we moved outside the cache area
            if (!IsInCache(currentPos))
            {
                Debug.Log("🛰 Outside of cache. Fetching more data...");
                StartCoroutine(FetchAndCacheData());
            }
        }
        if (Input.GetKeyDown(KeyCode.L))
    {
        LogDictionaries();
    }
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

    // ✅ If data exists in cache, apply it immediately
    if (cachedData.TryGetValue(gridPos, out GridData gridData))
    {
        cellScript.SetData(gridData);
        Debug.Log($"💾 Applied cached data to GridCell [{gridPos.x}, {gridPos.y}] - ID: {gridData.id}, Owner: {gridData.ownerOfTheGrid}");
    }

    activeCells.Add(gridPos, gridCell);
}


    bool IsInCache(Vector2Int position)
    {
        for (int x = -viewRadius * 2; x <= viewRadius * 2; x++)
        {
            for (int y = -viewRadius * 2; y <= viewRadius * 2; y++)
            {
                Vector2Int checkPos = new Vector2Int(position.x + x, position.y + y);
                if (!cachedData.ContainsKey(checkPos))
                {
                    return false;
                }
            }
        }
        return true;
    }

    IEnumerator FetchAndCacheData()
{
    Debug.Log("⏳ Waiting 1 seconds before fetching grid data...");
    yield return new WaitForSeconds(1); // Wait for 10 seconds

    if (cameraTransform == null || databaseManager == null)
    {
        Debug.LogError("❌ Missing required components.");
        yield break;
    }

    // Prepare a list to hold the visible grids
    List<Vector2Int> visibleGrids = new List<Vector2Int>();
    Vector2Int cameraGridPosition = GetGridPosition(cameraTransform.position);

    for (int x = -viewRadius * 2; x <= viewRadius * 2; x++)
    {
        for (int y = -viewRadius * 2; y <= viewRadius * 2; y++)
        {
            Vector2Int gridPos = new Vector2Int(cameraGridPosition.x + x, cameraGridPosition.y + y);

            if (!cachedData.ContainsKey(gridPos))
            {
                visibleGrids.Add(gridPos);
            }
        }
    }

    if (visibleGrids.Count == 0)
    {
        Debug.Log("🟢 All grids are already cached.");
        yield break;
    }

    Debug.Log($"📤 Fetching data for {visibleGrids.Count} grid positions...");
    yield return StartCoroutine(databaseManager.FetchGridData(visibleGrids, (gridDataList) =>
    {
        if (gridDataList == null)
        {
            Debug.LogError("❌ Fetch failed, gridDataList is null.");
            return;
        }

        foreach (var gridData in gridDataList)
        {
            Vector2Int gridPos = new Vector2Int(gridData.grid_x, gridData.grid_y);

            // ✅ Cache the data if it's not already there
            if (!cachedData.ContainsKey(gridPos))
            {
                cachedData[gridPos] = gridData;
                Debug.Log($"🗂 Cached data for Grid [{gridPos.x}, {gridPos.y}] - Owner: {gridData.ownerOfTheGrid}");
            }

            // ✅ If the cell is already active, we update it immediately
            if (activeCells.TryGetValue(gridPos, out GameObject cellObject))
            {
                GridCell cellScript = cellObject.GetComponent<GridCell>();
                cellScript.SetData(gridData);
                Debug.Log($"🔄 Updated active cell [{gridPos.x}, {gridPos.y}] with new data.");
            }
        }
    }));
}


public void LogDictionaries()
{
    Debug.Log($"🔍 Active Cells Count: {activeCells.Count}");
    Debug.Log("=== Active Cells ===");
    foreach (var cell in activeCells)
    {
        Debug.Log($"[Position: {cell.Key}] -> GameObject: {cell.Value.name}");
    }

    Debug.Log($"🔍 Cached Data Count: {cachedData.Count}");
    Debug.Log("=== Cached Data ===");
    foreach (var data in cachedData)
    {
        Debug.Log($"[Position: {data.Key}] -> GridData: ID: {data.Value.id}, Owner: {data.Value.ownerOfTheGrid}");
    }
    Debug.Log("====================");
}

}




