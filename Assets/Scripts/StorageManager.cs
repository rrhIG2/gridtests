using UnityEngine;
using System.Collections;

public class StorageManager : MonoBehaviour
{
    [SerializeField] private DatabaseManager databaseManager;

    [Header("Stored Resources")]
    [SerializeField] public float _wood { get; private set; }
    [SerializeField] public float _stone { get; private set; }
    [SerializeField] public float _gold { get; private set; }
    [SerializeField] public float _copper { get; private set; }
    [SerializeField] public float _coal { get; private set; }
    [SerializeField] public float _oil { get; private set; }
    [SerializeField] public float _uranium { get; private set; }
    [SerializeField] public float _food { get; private set; }
    [SerializeField] public float _water { get; private set; }
    [SerializeField] public float _iron { get; private set; }

    [Header("Mining Rates")]
    [SerializeField] public float _miningWood { get; private set; }
    [SerializeField] public float _miningStone { get; private set; }
    [SerializeField] public float _miningGold { get; private set; }
    [SerializeField] public float _miningCopper { get; private set; }
    [SerializeField] public float _miningCoal { get; private set; }
    [SerializeField] public float _miningOil { get; private set; }
    [SerializeField] public float _miningUranium { get; private set; }
    [SerializeField] public float _miningFood { get; private set; }
    [SerializeField] public float _miningWater { get; private set; }
    [SerializeField] public float _miningIron { get; private set; }

    public void RefreshStorage()
    {
        int userId = PlayerPrefs.GetInt("UserId", -1);
        if (userId < 0)
        {
            Debug.LogWarning("❌ Invalid user ID.");
            return;
        }

        Debug.Log("🔄 Calling RefreshStorage()");

        StartCoroutine(databaseManager.FetchUserStorage(userId, (data) =>
        {
            if (data == null)
            {
                Debug.LogError("❌ Failed to fetch storage.");
                return;
            }

            _wood = data.storage_actual_wood ?? 0f;
            _stone = data.storage_actual_stone ?? 0f;
            _gold = data.storage_actual_gold ?? 0f;
            _copper = data.storage_actual_copper ?? 0f;
            _coal = data.storage_actual_coal ?? 0f;
            _oil = data.storage_actual_oil ?? 0f;
            _uranium = data.storage_actual_uranium ?? 0f;
            _food = data.storage_actual_food ?? 0f;
            _water = data.storage_actual_water ?? 0f;
            _iron = data.storage_actual_iron ?? 0f;

            _miningWood = data.mining_value_wood ?? 0f;
            _miningStone = data.mining_value_stone ?? 0f;
            _miningGold = data.mining_value_gold ?? 0f;
            _miningCopper = data.mining_value_stone ?? 0f;
            _miningCoal = data.mining_value_coal ?? 0f;
            _miningOil = data.mining_value_oil ?? 0f;
            _miningUranium = data.mining_value_uranium ?? 0f;
            _miningFood = data.mining_value_food ?? 0f;
            _miningWater = data.mining_value_water ?? 0f;
            _miningIron = data.mining_value_iron ?? 0f;


            Debug.Log("✅ Storage updated.");
            // You can notify UI here to update if needed.
        }));
    }
}
