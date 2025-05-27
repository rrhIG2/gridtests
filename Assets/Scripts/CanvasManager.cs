using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class CanvasManager : MonoBehaviour
{
    [Header("UI Canvases")]
    [SerializeField] private Canvas _Tier1Canvas;
    [SerializeField] private Canvas _Tier1PotencialMaterialCanvas;

    [Header("Tier 1 Canvas")]
    [SerializeField] private TextMeshProUGUI _tier1CoordinatesText;
    [SerializeField] private TextMeshProUGUI _tier1OwnerText;
    [SerializeField] private Button _tier1PotencionalMaterialButton;
    [SerializeField] private Button _tier1BuyLandButton;
    [SerializeField] private Button _hideTier1CanvasButton;
    [SerializeField] private Button _tier2MaterialsButton;

    [Header("Tier 1 Potential Material Canvas")]
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

    [SerializeField] private Button _hidePotentialMaterialButton;


    private GridCell currentGridCell;

    [SerializeField] private DatabaseManager databaseManager;

    private bool _buyLandInProgress = false;


    private void Awake()
    {
        // Initialize the canvases
        _Tier1Canvas.enabled = false;
        _Tier1PotencialMaterialCanvas.enabled = false;
        /*
                // Set up button listeners
                _tier1PotencionalMaterialButton.onClick.AddListener(() => ShowPotentialMaterialCanvas());
                _tier1BuyLand.onClick.AddListener(() => BuyLand());*/
    }

    /// <summary>
    /// Displays the Grid Info UI with the given data from GridCell
    /// </summary>
    public void Show(GridCell gridCell)
    {
        currentGridCell = gridCell;

        // Populate UI elements with values from GridCell
        _Tier1Canvas.enabled = true;
        _Tier1PotencialMaterialCanvas.enabled = false;
        _tier1CoordinatesText.text = $"X = {gridCell.X} : Y = {gridCell.Y}";
        _tier1OwnerText.text = $"Owner: {gridCell._ownerNickname}";
        //_tier1PotencionalMaterialButton.onClick.RemoveAllListeners(); // remove this 
        _tier1PotencionalMaterialButton.onClick.AddListener(() => ShowPotentialMaterialCanvas());
        //_hideTier1CanvasButton.onClick.RemoveAllListeners();
        _hideTier1CanvasButton.onClick.AddListener(() => HideTier1Canvas());
        //_tier1BuyLandButton.onClick.RemoveAllListeners();
        _tier1BuyLandButton.onClick.AddListener(() => OnBuyLandButtonClicked());


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
        //_hidePotentialMaterialButton.onClick.RemoveAllListeners();
        _hidePotentialMaterialButton.onClick.AddListener(() => HidePotentialMaterialCanvas());

        //currentGridCell.DebugPrint();

        int currentUserId = PlayerPrefs.GetInt("UserId", -1);
        bool isOwner = currentUserId == currentGridCell.GerGridOwnerId();

        _tier1BuyLandButton.gameObject.SetActive(!isOwner);
        _tier2MaterialsButton.gameObject.SetActive(isOwner);

        // Production buttons visibility
        startProductionWood.gameObject.SetActive(isOwner);
        startProductionStone.gameObject.SetActive(isOwner);
        startProductionIron.gameObject.SetActive(isOwner);
        startProductionGold.gameObject.SetActive(isOwner);
        startProductionCopper.gameObject.SetActive(isOwner);
        startProductionCoal.gameObject.SetActive(isOwner);
        startProductionOil.gameObject.SetActive(isOwner);
        startProductionUranium.gameObject.SetActive(isOwner);
        startProductionFood.gameObject.SetActive(isOwner);
        startProductionWater.gameObject.SetActive(isOwner);

        // Set visuals if player is owner
        if (isOwner)
        {
            SetProductionButton(startProductionWood, currentGridCell.MaterialWoodIsMined);
            SetProductionButton(startProductionStone, currentGridCell.MaterialStoneIsMined);
            SetProductionButton(startProductionIron, currentGridCell.MaterialIronIsMined);
            SetProductionButton(startProductionGold, currentGridCell.MaterialGoldIsMined);
            SetProductionButton(startProductionCopper, currentGridCell.MaterialCopperIsMined);
            SetProductionButton(startProductionCoal, currentGridCell.MaterialCoalIsMined);
            SetProductionButton(startProductionOil, currentGridCell.MaterialOilIsMined);
            SetProductionButton(startProductionUranium, currentGridCell.MaterialUraniumIsMined);
            SetProductionButton(startProductionFood, currentGridCell.MaterialFoodIsMined);
            SetProductionButton(startProductionWater, currentGridCell.MaterialWaterIsMined);
        }

        // Hook up start production button actions
        startProductionWood.onClick.RemoveAllListeners();
        startProductionWood.onClick.AddListener(() => OnStartProductionClicked(MaterialType.Wood));

        startProductionStone.onClick.RemoveAllListeners();
        startProductionStone.onClick.AddListener(() => OnStartProductionClicked(MaterialType.Stone));

        startProductionIron.onClick.RemoveAllListeners();
        startProductionIron.onClick.AddListener(() => OnStartProductionClicked(MaterialType.Iron));

        startProductionGold.onClick.RemoveAllListeners();
        startProductionGold.onClick.AddListener(() => OnStartProductionClicked(MaterialType.Gold));

        startProductionCopper.onClick.RemoveAllListeners();
        startProductionCopper.onClick.AddListener(() => OnStartProductionClicked(MaterialType.Copper));

        startProductionCoal.onClick.RemoveAllListeners();
        startProductionCoal.onClick.AddListener(() => OnStartProductionClicked(MaterialType.Coal));

        startProductionOil.onClick.RemoveAllListeners();
        startProductionOil.onClick.AddListener(() => OnStartProductionClicked(MaterialType.Oil));

        startProductionUranium.onClick.RemoveAllListeners();
        startProductionUranium.onClick.AddListener(() => OnStartProductionClicked(MaterialType.Uranium));

        startProductionFood.onClick.RemoveAllListeners();
        startProductionFood.onClick.AddListener(() => OnStartProductionClicked(MaterialType.Food));

        startProductionWater.onClick.RemoveAllListeners();
        startProductionWater.onClick.AddListener(() => OnStartProductionClicked(MaterialType.Water));



    }



    private void ShowPotentialMaterialCanvas()
    {
        _Tier1Canvas.enabled = false;
        _Tier1PotencialMaterialCanvas.enabled = true;
    }

    private void HidePotentialMaterialCanvas()
    {
        _Tier1Canvas.enabled = true;
        _Tier1PotencialMaterialCanvas.enabled = false;
    }

    private void HideTier1Canvas()
    {
        _Tier1Canvas.enabled = false;
        _Tier1PotencialMaterialCanvas.enabled = false;
    }

    private void OnBuyLandButtonClicked()
    {
        if (_buyLandInProgress)
        {
            Debug.Log("🛑 BuyLand action already in progress. Ignoring repeated clicks.");
            return;
        }

        _buyLandInProgress = true;

        Debug.Log("🛒 BuyLand button pressed.");

        int playerId = PlayerPrefs.GetInt("UserId", -1);
        int gridId = currentGridCell.GetGridId(); // Or however you track it

        StartCoroutine(databaseManager.BuyLandRequest(playerId, gridId, (updatedData) =>
        {
            if (updatedData != null)
            {
                currentGridCell.SetData(updatedData);
                Debug.Log("✅ Land purchase complete.");
            }
            else
            {
                Debug.LogWarning("⚠️ Land purchase failed.");
            }

            _buyLandInProgress = false; // Allow button press again
        }));
    }

    // Helper function to set button visuals based on mining status
    void SetProductionButton(Button button, bool? isMined)
    {
        var colors = button.colors;
        if (isMined == true)
        {
            colors.normalColor = Color.red;
            button.GetComponentInChildren<TextMeshProUGUI>().text = "Already Producing";
        }
        else
        {
            colors.normalColor = Color.green;
            button.GetComponentInChildren<TextMeshProUGUI>().text = "Start Producing";
        }
        button.colors = colors;
    }

    private void OnStartProductionClicked(MaterialType material)
    {
        int playerId = PlayerPrefs.GetInt("UserId", -1);
        int gridId = currentGridCell.GetGridId();

        if (playerId < 0 || gridId <= 0)
        {
            Debug.LogError("❌ Invalid player ID or grid ID.");
            return;
        }

        Debug.Log($"⛏ Sending start production request for {material} on grid {gridId}");

        StartCoroutine(databaseManager.StartProductionRequest(playerId, gridId, material, (updatedData) =>
        {
            if (updatedData != null)
            {
                currentGridCell.SetData(updatedData);
                Show(currentGridCell); // Refresh UI
                Debug.Log($"✅ Production started for {material}.");
            }
            else
            {
                Debug.LogWarning("⚠️ Failed to start production.");
            }
        }));
    }


}
