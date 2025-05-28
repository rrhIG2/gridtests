using UnityEngine;

public class ResourceRefreshManager : MonoBehaviour
{
    [SerializeField] private GridSystem _gridSystem;
    [SerializeField] private StorageManager _storageManager;
    [SerializeField] private CanvasManager _canvasManager;

    private float timer = 0f;
    private float interval = 10f;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= interval)
        {
            timer = 0f;
            _gridSystem.RefreshVisibleGrids();
            _storageManager.RefreshStorage();
            _canvasManager.UpdateTier1UIMaterials();
        }
    }
}
