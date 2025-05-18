using UnityEngine;
using TMPro;

public class GridCell : MonoBehaviour
{
    [Header("Grid Coordinates")]
    [SerializeField] private int x;
    [SerializeField] private int y;

    [Header("Grid Data")]
    [SerializeField] private int idOfOwner;  
    [SerializeField] private string ownerNickname;

    // Material potentials and actual values
    [Header("Material Data")]
    [SerializeField] private double materialPotentialWood;
    [SerializeField] private double materialActualWood;

    [SerializeField] private double materialPotentialStone;
    [SerializeField] private double materialActualStone;

    [SerializeField] private double materialPotentialIron;
    [SerializeField] private double materialActualIron;

    [SerializeField] private double materialPotentialGold;
    [SerializeField] private double materialActualGold;

    [SerializeField] private double materialPotentialCopper;
    [SerializeField] private double materialActualCopper;

    [SerializeField] private double materialPotentialCoal;
    [SerializeField] private double materialActualCoal;

    [SerializeField] private double materialPotentialOil;
    [SerializeField] private double materialActualOil;

    [SerializeField] private double materialPotentialUranium;
    [SerializeField] private double materialActualUranium;

    [SerializeField] private double materialPotentialFood;
    [SerializeField] private double materialActualFood;

    [SerializeField] private double materialPotentialWater;
    [SerializeField] private double materialActualWater;

    [Header("Production Info")]
    [SerializeField] private double materialMining;
    [SerializeField] private string productionType;

    [Header("Label Settings")]
    private TextMeshPro label;
    private Renderer renderer;

    public int X => x;
    public int Y => y;

    /// <summary>
    /// Initializes the basic grid coordinates
    /// </summary>
    public void Initialize(int x, int y)
    {
        this.x = x;
        this.y = y;

        // Get the renderer for color manipulation
        renderer = GetComponent<Renderer>();
    }

    /// <summary>
    /// Called to set additional data received from the server
    /// </summary>
    /// <param name="data">GridData object containing id, x, y, and owner</param>
    public void SetData(GridData data)
    {
        idOfOwner = data.ownerOfTheGridId ?? 0;
        ownerNickname = data.ownerOfTheGridNickname;

        // Assign all materials
        materialPotentialWood = data.material_potential_wood;
        materialActualWood = data.material_actual_wood;

        materialPotentialStone = data.material_potential_stone;
        materialActualStone = data.material_actual_stone;

        materialPotentialIron = data.material_potential_iron;
        materialActualIron = data.material_actual_iron;

        materialPotentialGold = data.material_potential_gold;
        materialActualGold = data.material_actual_gold;

        materialPotentialCopper = data.material_potential_copper;
        materialActualCopper = data.material_actual_copper;

        materialPotentialCoal = data.material_potential_coal;
        materialActualCoal = data.material_actual_coal;

        materialPotentialOil = data.material_potential_oil;
        materialActualOil = data.material_actual_oil;

        materialPotentialUranium = data.material_potential_uranium;
        materialActualUranium = data.material_actual_uranium;

        materialPotentialFood = data.material_potential_food;
        materialActualFood = data.material_actual_food;

        materialPotentialWater = data.material_potential_water;
        materialActualWater = data.material_actual_water;

        // Production information
        materialMining = data.material_mining ?? 0;
    productionType = data.production_type ?? "None";
        // Create the label if it doesn't exist
        if (label == null)
        {
            GameObject newLabel = new GameObject("GridLabel");
            newLabel.transform.SetParent(transform);

            // Set Y position to 0.5 and flat
            newLabel.transform.localPosition = new Vector3(0, .75f, 0);
            newLabel.transform.localEulerAngles = new Vector3(90, 0, 0);

            label = newLabel.AddComponent<TextMeshPro>();
            label.fontSize = 2;
            label.alignment = TextAlignmentOptions.Center;
            label.color = Color.white;
        }

        // Update the label text
        label.text = $"ID: {idOfOwner}\nOwner: {ownerNickname}\n(x: {x}, y: {y})";

        // ✅ Change color based on ownership
        Debug.Log("Sesionid " + SessionData.UserId + " id " + idOfOwner);
        if (SessionData.UserId == idOfOwner)
        {
            SetColor(Color.green); // Owned by current user
        }
        else
        {
            SetColor(new Color(1f, 0.65f, 0f)); // Orange color
        }
    }

    /// <summary>
    /// Sets the color of the grid cell
    /// </summary>
    /// <param name="color">Color to set</param>
    private void SetColor(Color color)
    {
        if (renderer == null)
            renderer = GetComponent<Renderer>();

        if (renderer != null)
        {
            renderer.material.color = color;
        }
    }
}
