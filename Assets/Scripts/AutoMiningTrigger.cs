using UnityEngine;

public class AutoMiningTrigger : MonoBehaviour
{
    [SerializeField] private DatabaseManager databaseManager;
    private float timer = 0f;
    private float interval = 10f;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= interval)
        {
            timer = 0f;
            StartCoroutine(databaseManager.TriggerAutoMining(success => {
                if (!success)
                {
                    Debug.LogWarning("Auto-mining tick failed to execute.");
                }
            }));
        }
    }
}
