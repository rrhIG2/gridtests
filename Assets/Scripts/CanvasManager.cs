using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class CanvasManager : MonoBehaviour
{
    [Header("UI Canvases")]
    [SerializeField] private Canvas _tier1Header_Canvas;
    [SerializeField] private Canvas _tier1_Canvas;
    [SerializeField] private Canvas _tier1_PotentialMaterial_Canvas;
    [SerializeField] private Canvas _tier2_MaterialsGridProduction_Canvas;
    [SerializeField] private Canvas _tier1_MaterialsUserProduction_Canvas;

    [Header("Tier1 Header Canvas")]
    [SerializeField] private TextMeshProUGUI _tier1Header_UserNickname_Text;
    [SerializeField] private Button _tier1Header_ShowMaterials_Button;

    [Header("Tier1 Canvas")]
    [SerializeField] private TextMeshProUGUI _tier1_Coordinates_Text;
    [SerializeField] private TextMeshProUGUI _tier1_OwnerOfTheGrid_Text;
    [SerializeField] private Button _tier1_ShowPotencionalMaterial_Button;
    [SerializeField] private Button _tier1_BuyLand_Button;
    [SerializeField] private Button _tier2_ShowMaterialsProducing_Button;
    [SerializeField] private Button _tier1_HideCanvas_Button;


    [Header("Tier 1 Potential Material Canvas")]
    [SerializeField] private TextMeshProUGUI _tier1PotentialMaterial_GridCoordinates_Text;

    [SerializeField] private TextMeshProUGUI _tier1PotentialMaterial_Wood_Text;
    [SerializeField] private Button _tier1PotentialMateria_Wood_Button;

    [SerializeField] private TextMeshProUGUI _tier1PotentialMaterial_Stone_Text;
    [SerializeField] private Button _tier1PotentialMateria_Stone_Button;

    [SerializeField] private TextMeshProUGUI _tier1PotentialMaterial_Iron_Text;
    [SerializeField] private Button _tier1PotentialMateria_Iron_Button;

    [SerializeField] private TextMeshProUGUI _tier1PotentialMaterial_Gold_Text;
    [SerializeField] private Button _tier1PotentialMateria_Gold_Button;

    [SerializeField] private TextMeshProUGUI _tier1PotentialMaterial_Copper_Text;
    [SerializeField] private Button _tier1PotentialMateria_Copper_Button;

    [SerializeField] private TextMeshProUGUI _tier1PotentialMaterial_Coal_Text;
    [SerializeField] private Button _tier1PotentialMateria_Coal_Button;

    [SerializeField] private TextMeshProUGUI _tier1PotentialMaterial_Oil_Text;
    [SerializeField] private Button _tier1PotentialMateria_Oil_Button;

    [SerializeField] private TextMeshProUGUI _tier1PotentialMaterial_Uranium_Text;
    [SerializeField] private Button _tier1PotentialMateria_Uranium_Button;

    [SerializeField] private TextMeshProUGUI _tier1PotentialMaterial_Food_Text;
    [SerializeField] private Button _tier1PotentialMateria_Food_Button;

    [SerializeField] private TextMeshProUGUI _tier1PotentialMaterial_Water_Text;
    [SerializeField] private Button _tier1PotentialMateria_Water_Button;

    [SerializeField] private Button _tier1_PotentialMaterialHide_Button;

    [Header("Tier 2 Materials Grid Production Canvas")]
    [SerializeField] private TextMeshProUGUI _tier2MaterialsGridProduction_Coordinates_Text;
    [SerializeField] private TextMeshProUGUI _tier2MaterialsGridProduction_Wood_Text;
    [SerializeField] private TextMeshProUGUI _tier2MaterialsGridProduction_Stone_Text;
    [SerializeField] private TextMeshProUGUI _tier2MaterialsGridProduction_Iron_Text;
    [SerializeField] private TextMeshProUGUI _tier2MaterialsGridProduction_Gold_Text;
    [SerializeField] private TextMeshProUGUI _tier2MaterialsGridProduction_Copper_Text;
    [SerializeField] private TextMeshProUGUI _tier2MaterialsGridProduction_Coal_Text;
    [SerializeField] private TextMeshProUGUI _tier2MaterialsGridProduction_Oil_Text;
    [SerializeField] private TextMeshProUGUI _tier2MaterialsGridProduction_Uranium_Text;
    [SerializeField] private TextMeshProUGUI _tier2MaterialsGridProduction_Food_Text;
    [SerializeField] private TextMeshProUGUI _tier2MaterialsGridProduction_Water_Text;
    [SerializeField] private Button _tier2_MaterialsGridProductionHide_Button;



    [Header("Tier.1 Materials User Production Canvas")]

    [SerializeField] private TextMeshProUGUI _tier1MaterialsUserProduction_Coordinates_Text;
    [SerializeField] private TextMeshProUGUI _tier1MaterialsUserProduction_Wood_Text;
    [SerializeField] private TextMeshProUGUI _tier1MaterialsUserProduction_Stone_Text;
    [SerializeField] private TextMeshProUGUI _tier1MaterialsUserProduction_Iron_Text;
    [SerializeField] private TextMeshProUGUI _tier1MaterialsUserProduction_Gold_Text;
    [SerializeField] private TextMeshProUGUI _tier1MaterialsUserProduction_Copper_Text;
    [SerializeField] private TextMeshProUGUI _tier1MaterialsUserProduction_Coal_Text;
    [SerializeField] private TextMeshProUGUI _tier1MaterialsUserProduction_Oil_Text;
    [SerializeField] private TextMeshProUGUI _tier1MaterialsUserProduction_Uranium_Text;
    [SerializeField] private TextMeshProUGUI _tier1MaterialsUserProduction_Food_Text;
    [SerializeField] private TextMeshProUGUI _tier1MaterialsUserProduction_Water_Text;
    [SerializeField] private Button _tier1_MaterialsUserProductionHide_Button;
    private GridCell currentGridCell;

    [SerializeField] private DatabaseManager _databaseManager;
    [SerializeField] private StorageManager _storageManager;

    private bool _buyLandInProgress = false;


    private void Awake()
    {
        HideAllCanvases();
        _tier1Header_Canvas.enabled = true;

        string nickname = PlayerPrefs.GetString("Nickname");
        _tier1Header_UserNickname_Text.text = $"User: {nickname}";
        _tier1Header_ShowMaterials_Button.onClick.AddListener(() => ShowTier1UIMaterials());
        _tier1_MaterialsUserProductionHide_Button.onClick.AddListener(() => HideTier1UIMaterials());

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




        // Populate UI elements with values from GridCell
        HideAllCanvases();
        _tier1_Canvas.enabled = true;

        _tier1_Coordinates_Text.text = $"X = {gridCell.X} : Y = {gridCell.Y}";
        _tier1_OwnerOfTheGrid_Text.text = hasNoOwner ? "This land is not owned" : $"Owner: {gridCell._ownerNickname}";

        //Debug.Log($"Current User ID: {currentUserId}, Grid Owner ID: {gridOwnerId}, Can Buy: {hasNoOwner && !isOwner}");

        _tier1_ShowPotencionalMaterial_Button.onClick.AddListener(() => ShowPotentialMaterialCanvas());
        _tier1_HideCanvas_Button.onClick.AddListener(() => HideTier1Canvas());
        _tier1_BuyLand_Button.onClick.AddListener(() => OnBuyLandButtonClicked());
        _tier2_ShowMaterialsProducing_Button.onClick.AddListener(() => ShowTier2MaterialsCanvas());

        _tier2_MaterialsGridProductionHide_Button.onClick.AddListener(() => HideTier2MaterialsCanvas());////////////////////////////////////////


        _tier1PotentialMaterial_GridCoordinates_Text.text = $"X = {gridCell.X} : Y = {gridCell.Y}";
        _tier1PotentialMaterial_Wood_Text.text = $"Wood: {gridCell.MaterialPotentialWood}";
        _tier1PotentialMaterial_Stone_Text.text = $"Stone: {gridCell.MaterialPotentialStone}";
        _tier1PotentialMaterial_Iron_Text.text = $"Iron: {gridCell.MaterialPotentialIron}";
        _tier1PotentialMaterial_Gold_Text.text = $"Gold: {gridCell.MaterialPotentialGold}";
        _tier1PotentialMaterial_Copper_Text.text = $"Copper: {gridCell.MaterialPotentialCopper}";
        _tier1PotentialMaterial_Coal_Text.text = $"Coal: {gridCell.MaterialPotentialCoal}";
        _tier1PotentialMaterial_Oil_Text.text = $"Oil: {gridCell.MaterialPotentialOil}";
        _tier1PotentialMaterial_Uranium_Text.text = $"Uranium: {gridCell.MaterialPotentialUranium}";
        _tier1PotentialMaterial_Food_Text.text = $"Food: {gridCell.MaterialPotentialFood}";
        _tier1PotentialMaterial_Water_Text.text = $"Water: {gridCell.MaterialPotentialWater}";
        _tier1_PotentialMaterialHide_Button.onClick.AddListener(() => HidePotentialMaterialCanvas());

        currentGridCell.DebugPrint();

        _tier1_BuyLand_Button.interactable = hasNoOwner && !isOwner;

        _tier2_ShowMaterialsProducing_Button.gameObject.SetActive(isOwner);

        // Production buttons visibility
        _tier1PotentialMateria_Wood_Button.gameObject.SetActive(isOwner);
        _tier1PotentialMateria_Stone_Button.gameObject.SetActive(isOwner);
        _tier1PotentialMateria_Iron_Button.gameObject.gameObject.SetActive(isOwner);
        _tier1PotentialMateria_Gold_Button.gameObject.gameObject.SetActive(isOwner);
        _tier1PotentialMateria_Copper_Button.gameObject.gameObject.SetActive(isOwner);
        _tier1PotentialMateria_Coal_Button.gameObject.gameObject.SetActive(isOwner);
        _tier1PotentialMateria_Oil_Button.gameObject.gameObject.SetActive(isOwner);
        _tier1PotentialMateria_Uranium_Button.gameObject.gameObject.SetActive(isOwner);
        _tier1PotentialMateria_Food_Button.gameObject.gameObject.SetActive(isOwner);
        _tier1PotentialMateria_Water_Button.gameObject.gameObject.SetActive(isOwner);

        // Set visuals if player is owner
        if (isOwner)
        {
            SetProductionButton(_tier1PotentialMateria_Wood_Button, currentGridCell.MaterialWoodIsMined);
            SetProductionButton(_tier1PotentialMateria_Stone_Button, currentGridCell.MaterialStoneIsMined);
            SetProductionButton(_tier1PotentialMateria_Iron_Button, currentGridCell.MaterialIronIsMined);
            SetProductionButton(_tier1PotentialMateria_Gold_Button, currentGridCell.MaterialGoldIsMined);
            SetProductionButton(_tier1PotentialMateria_Copper_Button, currentGridCell.MaterialCopperIsMined);
            SetProductionButton(_tier1PotentialMateria_Coal_Button, currentGridCell.MaterialCoalIsMined);
            SetProductionButton(_tier1PotentialMateria_Oil_Button, currentGridCell.MaterialOilIsMined);
            SetProductionButton(_tier1PotentialMateria_Uranium_Button, currentGridCell.MaterialUraniumIsMined);
            SetProductionButton(_tier1PotentialMateria_Food_Button, currentGridCell.MaterialFoodIsMined);
            SetProductionButton(_tier1PotentialMateria_Water_Button, currentGridCell.MaterialWaterIsMined);
        }

        // Hook up start production button actions
        _tier1PotentialMateria_Wood_Button.onClick.AddListener(() => OnStartProductionClicked(MaterialType.Wood));

        _tier1PotentialMateria_Stone_Button.onClick.RemoveAllListeners();
        _tier1PotentialMateria_Stone_Button.onClick.AddListener(() => OnStartProductionClicked(MaterialType.Stone));

        _tier1PotentialMateria_Iron_Button.onClick.RemoveAllListeners();
        _tier1PotentialMateria_Iron_Button.onClick.AddListener(() => OnStartProductionClicked(MaterialType.Iron));

        _tier1PotentialMateria_Gold_Button.onClick.RemoveAllListeners();
        _tier1PotentialMateria_Gold_Button.onClick.AddListener(() => OnStartProductionClicked(MaterialType.Gold));

        _tier1PotentialMateria_Copper_Button.onClick.RemoveAllListeners();
        _tier1PotentialMateria_Copper_Button.onClick.AddListener(() => OnStartProductionClicked(MaterialType.Copper));

        _tier1PotentialMateria_Coal_Button.onClick.RemoveAllListeners();
        _tier1PotentialMateria_Coal_Button.onClick.AddListener(() => OnStartProductionClicked(MaterialType.Coal));

        _tier1PotentialMateria_Oil_Button.onClick.RemoveAllListeners();
        _tier1PotentialMateria_Oil_Button.onClick.AddListener(() => OnStartProductionClicked(MaterialType.Oil));

        _tier1PotentialMateria_Uranium_Button.onClick.RemoveAllListeners();
        _tier1PotentialMateria_Uranium_Button.onClick.AddListener(() => OnStartProductionClicked(MaterialType.Uranium));

        _tier1PotentialMateria_Food_Button.onClick.RemoveAllListeners();
        _tier1PotentialMateria_Food_Button.onClick.AddListener(() => OnStartProductionClicked(MaterialType.Food));

        _tier1PotentialMateria_Water_Button.onClick.RemoveAllListeners();
        _tier1PotentialMateria_Water_Button.onClick.AddListener(() => OnStartProductionClicked(MaterialType.Water));

    }



    private void ShowPotentialMaterialCanvas()
    {
        HideAllCanvases();
        _tier1_PotentialMaterial_Canvas.enabled = true;
    }

    private void HidePotentialMaterialCanvas()
    {
        HideAllCanvases();
        _tier1_Canvas.enabled = true;
    }

    private void HideTier1Canvas()
    {
        HideAllCanvases();
        _tier1Header_Canvas.enabled = true;
    }

    private void ShowTier2MaterialsCanvas()
    {
        HideAllCanvases();
        _tier2_MaterialsGridProduction_Canvas.enabled = true;
        _tier2MaterialsGridProduction_Coordinates_Text.text = $"X = {currentGridCell.X} : Y = {currentGridCell.Y}";
        UpdateTier2MaterialTexts();
    }

    private void HideTier2MaterialsCanvas()
    {
        HideAllCanvases();
        _tier1_Canvas.enabled = true;
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

        StartCoroutine(_databaseManager.BuyLandRequest(playerId, gridId, (updatedData) =>
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

        StartCoroutine(_databaseManager.StartProductionRequest(playerId, gridId, material, (updatedData) =>
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
        SetTier2MaterialText(_tier2MaterialsGridProduction_Wood_Text, currentGridCell.MaterialWoodIsMined, currentGridCell.MaterialActualWood, currentGridCell.MaterialWoodMiningValue, "Wood");
        SetTier2MaterialText(_tier2MaterialsGridProduction_Stone_Text, currentGridCell.MaterialStoneIsMined, currentGridCell.MaterialActualStone, currentGridCell.MaterialStoneMiningValue, "Stone");
        SetTier2MaterialText(_tier2MaterialsGridProduction_Iron_Text, currentGridCell.MaterialIronIsMined, currentGridCell.MaterialActualIron, currentGridCell.MaterialIronMiningValue, "Iron");
        SetTier2MaterialText(_tier2MaterialsGridProduction_Gold_Text, currentGridCell.MaterialGoldIsMined, currentGridCell.MaterialActualGold, currentGridCell.MaterialGoldMiningValue, "Gold");
        SetTier2MaterialText(_tier2MaterialsGridProduction_Copper_Text, currentGridCell.MaterialCopperIsMined, currentGridCell.MaterialActualCopper, currentGridCell.MaterialCopperMiningValue, "Copper");
        SetTier2MaterialText(_tier2MaterialsGridProduction_Coal_Text, currentGridCell.MaterialCoalIsMined, currentGridCell.MaterialActualCoal, currentGridCell.MaterialCoalMiningValue, "Coal");
        SetTier2MaterialText(_tier2MaterialsGridProduction_Oil_Text, currentGridCell.MaterialOilIsMined, currentGridCell.MaterialActualOil, currentGridCell.MaterialOilMiningValue, "Oil");
        SetTier2MaterialText(_tier2MaterialsGridProduction_Uranium_Text, currentGridCell.MaterialUraniumIsMined, currentGridCell.MaterialActualUranium, currentGridCell.MaterialUraniumMiningValue, "Uranium");
        SetTier2MaterialText(_tier2MaterialsGridProduction_Food_Text, currentGridCell.MaterialFoodIsMined, currentGridCell.MaterialActualFood, currentGridCell.MaterialFoodMiningValue, "Food");
        SetTier2MaterialText(_tier2MaterialsGridProduction_Water_Text, currentGridCell.MaterialWaterIsMined, currentGridCell.MaterialActualWater, currentGridCell.MaterialWaterMiningValue, "Water");
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


    private void HideTier1UIMaterials()
    {
        HideAllCanvases();
        _tier1Header_Canvas.enabled = true;
    }

    private void ShowTier1UIMaterials()
    {
        Debug.Log("Showing Tier 1 Materials UI");
        HideAllCanvases();
        _tier1_MaterialsUserProduction_Canvas.enabled = true;

        UpdateTier1UIMaterials();

    }

    public void UpdateTier1UIMaterials()
    {
        string nickname = PlayerPrefs.GetString("Nickname");
        _tier1MaterialsUserProduction_Coordinates_Text.text = $"User: {nickname}";

        _tier1MaterialsUserProduction_Wood_Text.text = $"Wood: +{_storageManager._miningWood}/10s | {_storageManager._wood}";
        _tier1MaterialsUserProduction_Stone_Text.text = $"Stone: +{_storageManager._miningStone}/10s | {_storageManager._stone}";
        _tier1MaterialsUserProduction_Iron_Text.text = $"Iron: +{_storageManager._miningIron}/10s | {_storageManager._iron}";
        _tier1MaterialsUserProduction_Gold_Text.text = $"Gold: +{_storageManager._miningGold}/10s | {_storageManager._gold}";
        _tier1MaterialsUserProduction_Copper_Text.text = $"Copper: +{_storageManager._miningCopper}/10s | {_storageManager._copper}";
        _tier1MaterialsUserProduction_Coal_Text.text = $"Coal: +{_storageManager._miningCoal}/10s | {_storageManager._coal}";
        _tier1MaterialsUserProduction_Oil_Text.text = $"Oil: +{_storageManager._miningOil}/10s | {_storageManager._oil}";
        _tier1MaterialsUserProduction_Uranium_Text.text = $"Uranium: +{_storageManager._miningUranium}/10s | {_storageManager._uranium}";
        _tier1MaterialsUserProduction_Food_Text.text = $"Food: +{_storageManager._miningFood}/10s | {_storageManager._food}";
        _tier1MaterialsUserProduction_Water_Text.text = $"Water: +{_storageManager._miningWater}/10s | {_storageManager._water}";
    }


    private void HideAllCanvases()
    {
        Debug.Log("Hiding all canvases");
        _tier1_Canvas.enabled = false;
        _tier1_PotentialMaterial_Canvas.enabled = false;
        _tier2_MaterialsGridProduction_Canvas.enabled = false;
        _tier1Header_Canvas.enabled = false;
        _tier1_MaterialsUserProduction_Canvas.enabled = false;
    }
}
