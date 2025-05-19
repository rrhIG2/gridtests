using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GridInfoUI : MonoBehaviour
{
    [Header("UI Elements")]
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

    [SerializeField] private Canvas infoCanvas;

    private GridCell currentGridCell;

    private void Awake()
    {
        if (infoCanvas == null)
        {
            infoCanvas = GetComponent<Canvas>();
        }

        // Make sure the UI is not visible at the start
        infoCanvas.enabled = false;

        // Assign button listeners
        startProductionWood.onClick.AddListener(() => OnStartProductionButtonClick("wood"));
        startProductionStone.onClick.AddListener(() => OnStartProductionButtonClick("stone"));
        startProductionIron.onClick.AddListener(() => OnStartProductionButtonClick("iron"));
        startProductionGold.onClick.AddListener(() => OnStartProductionButtonClick("gold"));
        startProductionCopper.onClick.AddListener(() => OnStartProductionButtonClick("copper"));
        startProductionCoal.onClick.AddListener(() => OnStartProductionButtonClick("coal"));
        startProductionOil.onClick.AddListener(() => OnStartProductionButtonClick("oil"));
        startProductionUranium.onClick.AddListener(() => OnStartProductionButtonClick("uranium"));
        startProductionFood.onClick.AddListener(() => OnStartProductionButtonClick("food"));
        startProductionWater.onClick.AddListener(() => OnStartProductionButtonClick("water"));
    }

    /// <summary>
    /// Displays the Grid Info UI with the given data from GridCell
    /// </summary>
    public void Show(GridCell gridCell)
    {
        currentGridCell = gridCell;
        infoCanvas.enabled = true;

        // Populate UI elements with values from GridCell
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
    public void OnStartProductionButtonClick(string material)
    {
        if (currentGridCell == null)
        {
            Debug.LogError("⚠️ No Grid Data available to start production.");
            return;
        }

        Debug.Log($"🏭 Starting production on Grid ID: {currentGridCell.X}, {currentGridCell.Y} for Material: {material}");

        // 🚀 Call the backend or logic to start production here
    }
}
