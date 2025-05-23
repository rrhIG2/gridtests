using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System;

public class GridCell : MonoBehaviour
{
    [Header("Grid Coordinates")]
    [SerializeField] private int x;
    [SerializeField] private int y;

    [Header("Grid Data")]
    [SerializeField] private int idOfOwner;
    [SerializeField] private string ownerNickname;

    [Header("Material Data")]
    public double MaterialPotentialWood { get; private set; }
    public double MaterialActualWood { get; private set; }
    
    public double MaterialPotentialStone { get; private set; }
    public double MaterialActualStone { get; private set; }
    
    public double MaterialPotentialIron { get; private set; }
    public double MaterialActualIron { get; private set; }
    
    public double MaterialPotentialGold { get; private set; }
    public double MaterialActualGold { get; private set; }
    
    public double MaterialPotentialCopper { get; private set; }
    public double MaterialActualCopper { get; private set; }
    
    public double MaterialPotentialCoal { get; private set; }
    public double MaterialActualCoal { get; private set; }
    
    public double MaterialPotentialOil { get; private set; }
    public double MaterialActualOil { get; private set; }
    
    public double MaterialPotentialUranium { get; private set; }
    public double MaterialActualUranium { get; private set; }
    
    public double MaterialPotentialFood { get; private set; }
    public double MaterialActualFood { get; private set; }
    
    public double MaterialPotentialWater { get; private set; }
    public double MaterialActualWater { get; private set; }

    [Header("Production Info")]
    [SerializeField] public double materialMining;
    [SerializeField] public GridInfoUI.MaterialType productionType;

    [Header("Label Settings")]
    private TextMeshPro label;
    private Renderer renderer;

    // Reference to the UI
    private GridInfoUI gridInfoUI;

    public int X => x;
    public int Y => y;

    public void Initialize(int x, int y)
    {
        this.x = x;
        this.y = y;
        renderer = GetComponent<Renderer>();
        gridInfoUI = FindObjectOfType<GridInfoUI>();
    }

    public void SetData(GridData data)
    {
        idOfOwner = data.ownerOfTheGridId ?? 0;
        ownerNickname = data.ownerOfTheGridNickname;

        // Assign all materials and their actual amounts
        MaterialPotentialWood = data.material_potential_wood;
        MaterialActualWood = data.material_actual_wood;

        MaterialPotentialStone = data.material_potential_stone;
        MaterialActualStone = data.material_actual_stone;

        MaterialPotentialIron = data.material_potential_iron;
        MaterialActualIron = data.material_actual_iron;

        MaterialPotentialGold = data.material_potential_gold;
        MaterialActualGold = data.material_actual_gold;

        MaterialPotentialCopper = data.material_potential_copper;
        MaterialActualCopper = data.material_actual_copper;

        MaterialPotentialCoal = data.material_potential_coal;
        MaterialActualCoal = data.material_actual_coal;

        MaterialPotentialOil = data.material_potential_oil;
        MaterialActualOil = data.material_actual_oil;

        MaterialPotentialUranium = data.material_potential_uranium;
        MaterialActualUranium = data.material_actual_uranium;

        MaterialPotentialFood = data.material_potential_food;
        MaterialActualFood = data.material_actual_food;

        MaterialPotentialWater = data.material_potential_water;
        MaterialActualWater = data.material_actual_water;

        materialMining = data.material_mining ?? 0;

        // ✅ Parse the string to the enum
        if (Enum.TryParse(data.production_type, true, out GridInfoUI.MaterialType parsedType))
        {
            productionType = parsedType;
        }
        else
        {
            Debug.LogWarning($"⚠️ Could not parse production type: {data.production_type}. Defaulting to None.");
            productionType = GridInfoUI.MaterialType.None; // Assuming "None" exists in your enum
        }
        
        // ✅ Change color based on ownership
        SetColor(SessionData.UserId == idOfOwner ? Color.green : new Color(1f, 0.65f, 0f));
    }

    private void SetColor(Color color)
    {
        if (renderer == null) renderer = GetComponent<Renderer>();
        if (renderer != null) renderer.material.color = color;
    }


    /// <summary>
    /// Detects a touch or click event on the grid cell and opens the UI
    /// </summary>
    /*public void OnPointerClick(PointerEventData eventData)
    {
        if (gridInfoUI != null)
        {
            Debug.Log($"📌 Clicked on GridCell [{x}, {y}]");
            gridInfoUI.Show(this);
        }
    }*/
    
void Update()
{
    if (Input.GetMouseButtonDown(0))
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            GridCell gridCell = hit.collider.GetComponent<GridCell>();
            if (gridCell != null && gridInfoUI != null)
            {
                Debug.Log($"📌 Click detected on: {gridCell.gameObject.name}");
                gridInfoUI.Show(gridCell);
            }
        }
    }
}


}
