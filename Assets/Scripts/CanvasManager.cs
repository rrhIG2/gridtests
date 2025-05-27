using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class CanvasManager : MonoBehaviour
{
    [Header("UI Canvases")]
    [SerializeField] private Canvas _Tier1Canvas;
    [SerializeField] private Canvas _Tier1PotencialMaterialCanvas;
    [SerializeField] private Canvas _Tier2MaterialsCanvas;

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

    [Header("Tier 2 Materials Canvas")]
    [SerializeField] private TextMeshProUGUI _tier2CoordinatesText;
    [SerializeField] private TextMeshProUGUI _tier2WoodText;
    [SerializeField] private TextMeshProUGUI _tier2StoneText;
    [SerializeField] private TextMeshProUGUI _tier2IronText;
    [SerializeField] private TextMeshProUGUI _tier2GoldText;
    [SerializeField] private TextMeshProUGUI _tier2CopperText;
    [SerializeField] private TextMeshProUGUI _tier2CoalText;
    [SerializeField] private TextMeshProUGUI _tier2OilText;
    [SerializeField] private TextMeshProUGUI _tier2UraniumText;
    [SerializeField] private TextMeshProUGUI _tier2FoodText;
    [SerializeField] private TextMeshProUGUI _tier2WaterText;
    [SerializeField] private Button _tier2HideButton;

    private GridCell currentGridCell;

    [SerializeField] private DatabaseManager databaseManager;

    private bool _buyLandInProgress = false;


    private void Awake()
    {
        // Initialize the canvases
        _Tier1Canvas.enabled = false;
        _Tier1PotencialMaterialCanvas.enabled = false;
        _Tier2MaterialsCanvas.enabled = false;
        
    }

    /// <summary>
    /// Displays the Grid Info UI with the given data from GridCell
    /// </summary>
    public void Show(GridCell gridCell)
    {

        currentGridCell = gridCell;

        int currentUserId = PlayerPrefs.GetInt("UserId", -1);
        int gridOwnerId = currentGridCell.GerGridOwnerId();
        bool isOwner = currentUserId == gridOwnerId;
        bool hasNoOwner = gridOwnerId == 0;

        _tier1OwnerText.text = currentGridCell._ownerNickname;


        // Populate UI elements with values from GridCell
        _Tier1Canvas.enabled = true;
        _Tier1PotencialMaterialCanvas.enabled = false;
        _tier1CoordinatesText.text = $"X = {gridCell.X} : Y = {gridCell.Y}";
        Debug.Log($"Current User ID: {currentUserId}, Grid Owner ID: {gridOwnerId}, Can Buy: {hasNoOwner && !isOwner}");        //_tier1PotencionalMaterialButton.onClick.RemoveAllListeners(); // remove this 
        _tier1PotencionalMaterialButton.onClick.AddListener(() => ShowPotentialMaterialCanvas());
        _hideTier1CanvasButton.onClick.AddListener(() => HideTier1Canvas());
        _tier1BuyLandButton.onClick.AddListener(() => OnBuyLandButtonClicked());
        _tier2MaterialsButton.onClick.RemoveAllListeners();
        _tier2MaterialsButton.onClick.AddListener(() => ShowTier2MaterialsCanvas());
        _tier2HideButton.onClick.RemoveAllListeners();
        _tier2HideButton.onClick.AddListener(() => HideTier2MaterialsCanvas());


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
        _hidePotentialMaterialButton.onClick.AddListener(() => HidePotentialMaterialCanvas());

        //currentGridCell.DebugPrint();

        _tier1BuyLandButton.interactable = hasNoOwner && !isOwner;

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
        _Tier2MaterialsCanvas.enabled = false;
    }

    private void HidePotentialMaterialCanvas()
    {
        _Tier1Canvas.enabled = true;
        _Tier1PotencialMaterialCanvas.enabled = false;
        _Tier2MaterialsCanvas.enabled = false;
    }

    private void HideTier1Canvas()
    {
        _Tier1Canvas.enabled = false;
        _Tier1PotencialMaterialCanvas.enabled = false;
        _Tier2MaterialsCanvas.enabled = false;
    }

    private void ShowTier2MaterialsCanvas()
    {
        _Tier1Canvas.enabled = false;
        _Tier1PotencialMaterialCanvas.enabled = false;
        _Tier2MaterialsCanvas.enabled = true;

        _tier2CoordinatesText.text = $"X = {currentGridCell.X} : Y = {currentGridCell.Y}";
        UpdateTier2MaterialTexts(); 
    }

    private void HideTier2MaterialsCanvas()
    {
        _Tier1Canvas.enabled = true;
        _Tier1PotencialMaterialCanvas.enabled = false;
        _Tier2MaterialsCanvas.enabled = false;
    }

    private void OnBuyLandButtonClicked()
    {
        if (_buyLandInProgress) return;

        int playerId = PlayerPrefs.GetInt("UserId", -1);
        int gridId = currentGridCell.GetGridId();
        int gridOwnerId = currentGridCell.GerGridOwnerId();

        if (playerId < 0 || gridId <= 0 || gridOwnerId != 0)
        {
            Debug.LogWarning("⛔ Cannot buy land: either already owned or invalid data.");
            return;
        }

        _buyLandInProgress = true;
        Debug.Log("🛒 BuyLand button pressed.");

        StartCoroutine(databaseManager.BuyLandRequest(playerId, gridId, (updatedData) =>
        {
            if (updatedData != null)
            {
                currentGridCell.SetData(updatedData);
                Debug.Log("✅ Land purchase complete.");
                Show(currentGridCell); 
            }
            else
            {
                Debug.LogWarning("⚠️ Land purchase failed.");
            }

            _buyLandInProgress = false;
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

    private void UpdateTier2MaterialTexts()
    {
        SetTier2MaterialText(_tier2WoodText, currentGridCell.MaterialWoodIsMined, currentGridCell.MaterialActualWood, currentGridCell.MaterialWoodMiningValue, "Wood");
        SetTier2MaterialText(_tier2StoneText, currentGridCell.MaterialStoneIsMined, currentGridCell.MaterialActualStone, currentGridCell.MaterialStoneMiningValue, "Stone");
        SetTier2MaterialText(_tier2IronText, currentGridCell.MaterialIronIsMined, currentGridCell.MaterialActualIron, currentGridCell.MaterialIronMiningValue, "Iron");
        SetTier2MaterialText(_tier2GoldText, currentGridCell.MaterialGoldIsMined, currentGridCell.MaterialActualGold, currentGridCell.MaterialGoldMiningValue, "Gold");
        SetTier2MaterialText(_tier2CopperText, currentGridCell.MaterialCopperIsMined, currentGridCell.MaterialActualCopper, currentGridCell.MaterialCopperMiningValue, "Copper");
        SetTier2MaterialText(_tier2CoalText, currentGridCell.MaterialCoalIsMined, currentGridCell.MaterialActualCoal, currentGridCell.MaterialCoalMiningValue, "Coal");
        SetTier2MaterialText(_tier2OilText, currentGridCell.MaterialOilIsMined, currentGridCell.MaterialActualOil, currentGridCell.MaterialOilMiningValue, "Oil");
        SetTier2MaterialText(_tier2UraniumText, currentGridCell.MaterialUraniumIsMined, currentGridCell.MaterialActualUranium, currentGridCell.MaterialUraniumMiningValue, "Uranium");
        SetTier2MaterialText(_tier2FoodText, currentGridCell.MaterialFoodIsMined, currentGridCell.MaterialActualFood, currentGridCell.MaterialFoodMiningValue, "Food");
        SetTier2MaterialText(_tier2WaterText, currentGridCell.MaterialWaterIsMined, currentGridCell.MaterialActualWater, currentGridCell.MaterialWaterMiningValue, "Water");
    }

    private void SetTier2MaterialText(TextMeshProUGUI textField, bool? isMined, float? actualValue, float miningValue, string materialName)
    {
        if (isMined == true)
        {
            textField.color = Color.white;
            textField.text = $"{materialName}: {actualValue} (Mining rate: {miningValue} every 10s)";
        }
        else
        {
            textField.color = Color.red;
            textField.text = $"We are not mining this material.";
        }
    }

}
