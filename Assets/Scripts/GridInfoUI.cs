using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class GridInfoUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI GridCoordinates;
    [SerializeField] private TextMeshProUGUI potentialValueOfWood;
    [SerializeField] private Button startProductionWood;

    [SerializeField] private TextMeshProUGUI potentialValueOfStone;
    [SerializeField] private Button startProductionStone;

    [SerializeField] private TextMeshProUGUI potentialValueOfIron;
    [SerializeField] private Button startProductionIron;

    [SerializeField] private TextMeshProUGUI potentialValueOfGold;
    [SerializeField] private Button startProductionGold;

    [SerializeField] private TextMeshProUGUI potentialValueOfCopper;
    [SerializeField] private Button startProductionCopper;

    [SerializeField] private TextMeshProUGUI potentialValueOfCoal;
    [SerializeField] private Button startProductionCoal;

    [SerializeField] private TextMeshProUGUI potentialValueOfOil;
    [SerializeField] private Button startProductionOil;

    [SerializeField] private TextMeshProUGUI potentialValueOfUranium;
    [SerializeField] private Button startProductionUranium;

    [SerializeField] private TextMeshProUGUI potentialValueOfFood;
    [SerializeField] private Button startProductionFood;

    [SerializeField] private TextMeshProUGUI potentialValueOfWater;
    [SerializeField] private Button startProductionWater;

    [SerializeField] private Button tier_3;


    [SerializeField] private Canvas infoCanvas;

    private GridCell currentGridCell;

    [SerializeField] private DatabaseManager databaseManager;

    public enum MaterialType
    {
        None,
        Wood,
        Stone,
        Iron,
        Gold,
        Copper,
        Coal,
        Oil,
        Uranium,
        Food,
        Water,
        Tier_3
    }

    public int current_X;
    public int current_Y;


    private void Awake()
    {
        if (infoCanvas == null)
        {
            infoCanvas = GetComponent<Canvas>();
        }

        // Make sure the UI is not visible at the start
        infoCanvas.enabled = false;

        // Assign button listeners
        startProductionWood.onClick.AddListener(() => OnStartProductionButtonClick(MaterialType.Wood));
        startProductionStone.onClick.AddListener(() => OnStartProductionButtonClick(MaterialType.Stone));
        startProductionIron.onClick.AddListener(() => OnStartProductionButtonClick(MaterialType.Iron));
        startProductionGold.onClick.AddListener(() => OnStartProductionButtonClick(MaterialType.Gold));
        startProductionCopper.onClick.AddListener(() => OnStartProductionButtonClick(MaterialType.Copper));
        startProductionCoal.onClick.AddListener(() => OnStartProductionButtonClick(MaterialType.Coal));
        startProductionOil.onClick.AddListener(() => OnStartProductionButtonClick(MaterialType.Oil));
        startProductionUranium.onClick.AddListener(() => OnStartProductionButtonClick(MaterialType.Uranium));
        startProductionFood.onClick.AddListener(() => OnStartProductionButtonClick(MaterialType.Food));
        startProductionWater.onClick.AddListener(() => OnStartProductionButtonClick(MaterialType.Water));
        tier_3.onClick.AddListener(() => OnStartProductionButtonClick(MaterialType.Tier_3));

    }

    /// <summary>
    /// Displays the Grid Info UI with the given data from GridCell
    /// </summary>
    public void Show(GridCell gridCell)
    {
        currentGridCell = gridCell;
        infoCanvas.enabled = true;

        // Populate UI elements with values from GridCell
        GridCoordinates.text = $"X = {gridCell.X} : Y = {gridCell.Y}";
        potentialValueOfWood.text = $"Wood: {gridCell.MaterialPotentialWood}";
        potentialValueOfStone.text = $"Stone: {gridCell.MaterialPotentialStone}";
        potentialValueOfIron.text = $"Iron: {gridCell.MaterialPotentialIron}";
        potentialValueOfGold.text = $"Gold: {gridCell.MaterialPotentialGold}";
        potentialValueOfCopper.text = $"Copper: {gridCell.MaterialPotentialCopper}";
        potentialValueOfCoal.text = $"Coal: {gridCell.MaterialPotentialCoal}";
        potentialValueOfOil.text = $"Oil: {gridCell.MaterialPotentialOil}";
        potentialValueOfUranium.text = $"Uranium: {gridCell.MaterialPotentialUranium}";
        potentialValueOfFood.text = $"Food: {gridCell.MaterialPotentialFood}";
        potentialValueOfWater.text = $"Water: {gridCell.MaterialPotentialWater}";

        current_X = gridCell.X;
        current_Y = gridCell.Y;
    }

    /// <summary>
    /// Hides the Grid Info UI
    /// </summary>
    public void Hide()
    {
        infoCanvas.enabled = false;
    }

    /// <summary>
    /// Called when the Start Production button is clicked
    /// </summary>
    public void OnStartProductionButtonClick(MaterialType material)
{
    int xCoordinate = current_X;
    int yCoordinate = current_Y;
    int playerId = SessionData.UserId;

    if (currentGridCell == null)
    {
        Debug.LogError("⚠️ No Grid Data available to start production.");
        return;
    }

    Debug.Log($"🏭 Starting production on Grid Coordinates: X={xCoordinate}, Y={yCoordinate} for Material: {material}");

    // 🚀 Call the backend to start production
    double miningRate = (material == MaterialType.Tier_3) ? 0 : 10;

    StartCoroutine(databaseManager.SendStartProductionRequest(xCoordinate, yCoordinate, playerId, material, miningRate));
}

}
