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


}
