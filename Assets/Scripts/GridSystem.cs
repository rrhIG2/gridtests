using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

public class GridSystem : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private float _cellSize = 1f;
    [SerializeField] private int _viewRadius = 5;
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private GameObject _gridCellPrefab;
    [SerializeField] private Transform _baseTransform;
    [SerializeField] private Transform _gridParent;

    [Header("References")]
    [SerializeField] private DatabaseManager _databaseManager;

    private Dictionary<Vector2Int, GameObject> _activeCells = new();
    private Dictionary<Vector2Int, GridData> _cachedData = new();
    private Vector2Int _lastCameraPosition;

    private void Start()
    {
        if (_gridParent == null)
        {
            GameObject gridParentObject = new("GridCells");
            _gridParent = gridParentObject.transform;
        }

        _lastCameraPosition = GetGridPosition(_cameraTransform.position);
        UpdateVisibleCells();
        StartCoroutine(FetchAndCacheData());
    }

    private void Update()
    {
        Vector2Int currentPos = GetGridPosition(_cameraTransform.position);

        if (currentPos != _lastCameraPosition)
        {
            UpdateVisibleCells();
            _lastCameraPosition = currentPos;

            if (!IsInCache(currentPos))
            {
                Debug.Log("🛰 Outside of cache. Fetching more data...");
                StartCoroutine(FetchAndCacheData());
            }
        }

        if (Input.GetKeyDown(KeyCode.L)) LogDictionaries();
        if (Input.GetKeyDown(KeyCode.K)) LogActiveCells();

    }

    private void UpdateVisibleCells()
    {
        Vector2Int cameraGridPos = GetGridPosition(_cameraTransform.position);
        Vector2Int baseGridPos = GetGridPosition(_baseTransform.position);

        for (int x = -_viewRadius; x <= _viewRadius; x++)
        {
            for (int y = -_viewRadius; y <= _viewRadius; y++)
            {
                Vector2Int gridPos = new(cameraGridPos.x + x, cameraGridPos.y + y);

                if (!_activeCells.ContainsKey(gridPos))
                    CreateGridCell(gridPos, baseGridPos);
            }
        }
    }

    private Vector2Int GetGridPosition(Vector3 worldPos)
    {
        int x = Mathf.FloorToInt(worldPos.x / _cellSize);
        int y = Mathf.FloorToInt(worldPos.z / _cellSize);
        return new Vector2Int(x, y);
    }

    private void CreateGridCell(Vector2Int gridPos, Vector2Int baseGridPos)
    {
        Vector3 worldPos = new(gridPos.x * _cellSize, 0, gridPos.y * _cellSize);
        GameObject cell = Instantiate(_gridCellPrefab, worldPos, Quaternion.identity, _gridParent);
        cell.name = $"GridCell_{gridPos.x}_{gridPos.y}";

        int localX = gridPos.x - baseGridPos.x;
        int localY = gridPos.y - baseGridPos.y;

        GridCell script = cell.GetComponent<GridCell>();
        script.Initialize(localX, localY);

        if (_cachedData.TryGetValue(gridPos, out GridData data))
        {
            script.SetData(data);
            Debug.Log($"💾 Applied cached data to GridCell [{gridPos.x}, {gridPos.y}] - ID: {data.ownerOfTheGridId}, Owner: {data.ownerOfTheGridNickname}");
        }

        _activeCells[gridPos] = cell;
    }

    private bool IsInCache(Vector2Int pos)
    {
        for (int x = -_viewRadius * 2; x <= _viewRadius * 2; x++)
        {
            for (int y = -_viewRadius * 2; y <= _viewRadius * 2; y++)
            {
                Vector2Int check = new(pos.x + x, pos.y + y);
                if (!_cachedData.ContainsKey(check)) return false;
            }
        }
        return true;
    }

    private IEnumerator FetchAndCacheData()
    {
        Debug.Log("⏳ Waiting 1 second before fetching grid data...");
        yield return new WaitForSeconds(1);

        if (_cameraTransform == null || _databaseManager == null)
        {
            Debug.LogError("❌ Missing required components.");
            yield break;
        }

        List<Vector2Int> fetchList = new();
        Vector2Int camPos = GetGridPosition(_cameraTransform.position);

        for (int x = -_viewRadius * 2; x <= _viewRadius * 2; x++)
        {
            for (int y = -_viewRadius * 2; y <= _viewRadius * 2; y++)
            {
                Vector2Int pos = new(camPos.x + x, camPos.y + y);
                if (!_cachedData.ContainsKey(pos)) fetchList.Add(pos);
            }
        }

        if (fetchList.Count == 0)
        {
            Debug.Log("🟢 All grids are already cached.");
            yield break;
        }

        Debug.Log($"📤 Fetching data for {fetchList.Count} grid positions...");
        yield return StartCoroutine(_databaseManager.FetchGridData(fetchList, (gridList) =>
        {
            if (gridList == null)
            {
                Debug.LogError("❌ Fetch failed, gridDataList is null.");
                return;
            }

            foreach (var data in gridList)
            {
                Vector2Int pos = new(data.grid_x, data.grid_y);
                if (!_cachedData.ContainsKey(pos)) _cachedData[pos] = data;

                if (_activeCells.TryGetValue(pos, out GameObject obj))
                {
                    GridCell script = obj.GetComponent<GridCell>();
                    script.SetData(data);
                    Debug.Log($"🔄 Updated active cell [{pos.x}, {pos.y}] with new data.");
                }
            }
        }));
    }

    public void LogDictionaries()
    {
        Debug.Log($"🔍 Active Cells Count: {_activeCells.Count}");
        foreach (var cell in _activeCells)
            Debug.Log($"[Pos: {cell.Key}] -> GameObject: {cell.Value.name}");

        Debug.Log($"🔍 Cached Data Count: {_cachedData.Count}");
        foreach (var data in _cachedData)
            Debug.Log($"[Pos: {data.Key}] -> GridData: ID: {data.Value.id}, Owner: {data.Value.ownerOfTheGridId}");
    }

    private void LogActiveCells()
    {
        Debug.Log($"🔎 Active Cells ({_activeCells.Count} total):");

        foreach (var pair in _activeCells)
        {
            GridCell script = pair.Value.GetComponent<GridCell>();
            if (script != null)
            {
               // Debug.Log($"📌 Grid Position: {pair.Key} | Production: {script.productionType} | Rate: {script.materialMining} | Wood: {script.MaterialActualWood}");
            }
        }
    }

    
}
