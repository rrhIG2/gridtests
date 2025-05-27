using UnityEngine;
using TMPro;
using System;

public class GridCell : MonoBehaviour
{
    [SerializeField] private int _id;
    [Header("Coordinates")]
    [SerializeField] private int _x;
    [SerializeField] private int _y;
    [SerializeField] private int? _latSec;
    [SerializeField] private int? _lonSec;

    [Header("Grid Data")]
    [SerializeField] private int _idOfOwner;
    [SerializeField] public string _ownerNickname { get; private set; }

    [Header("Material Data")]
    public float? MaterialPotentialWood { get; private set; }
    public float? MaterialActualWood { get; private set; }
    public bool? MaterialWoodIsMined { get; private set; }
    public float MaterialWoodMiningValue { get; private set; }

    public float? MaterialPotentialStone { get; private set; }
    public float? MaterialActualStone { get; private set; }
    public bool? MaterialStoneIsMined { get; private set; }
    public float MaterialStoneMiningValue { get; private set; }

    public float? MaterialPotentialGold { get; private set; }
    public float? MaterialActualGold { get; private set; }
    public bool? MaterialGoldIsMined { get; private set; }
    public float MaterialGoldMiningValue { get; private set; }

    public float? MaterialPotentialCopper { get; private set; }
    public float? MaterialActualCopper { get; private set; }
    public bool? MaterialCopperIsMined { get; private set; }
    public float MaterialCopperMiningValue { get; private set; }

    public float? MaterialPotentialCoal { get; private set; }
    public float? MaterialActualCoal { get; private set; }
    public bool? MaterialCoalIsMined { get; private set; }
    public float MaterialCoalMiningValue { get; private set; }

    public float? MaterialPotentialOil { get; private set; }
    public float? MaterialActualOil { get; private set; }
    public bool? MaterialOilIsMined { get; private set; }
    public float MaterialOilMiningValue { get; private set; }

    public float? MaterialPotentialUranium { get; private set; }
    public float? MaterialActualUranium { get; private set; }
    public bool? MaterialUraniumIsMined { get; private set; }
    public float MaterialUraniumMiningValue { get; private set; }

    public float? MaterialPotentialFood { get; private set; }
    public float? MaterialActualFood { get; private set; }
    public bool? MaterialFoodIsMined { get; private set; }
    public float MaterialFoodMiningValue { get; private set; }

    public float? MaterialPotentialWater { get; private set; }
    public float? MaterialActualWater { get; private set; }
    public bool? MaterialWaterIsMined { get; private set; }
    public float MaterialWaterMiningValue { get; private set; }

    public float? MaterialPotentialIron { get; private set; }
    public float? MaterialActualIron { get; private set; }
    public bool? MaterialIronIsMined { get; private set; }
    public float MaterialIronMiningValue { get; private set; }

    [Header("Label Settings")]
    private TextMeshPro _label;
    private Renderer _renderer;
    private CanvasManager _canvasManager;

    public int X => _x;
    public int Y => _y;

    private bool _clickInProgress = false; // Flag to prevent multiple clicks

    public void Initialize(int x, int y)
    {
        _x = x;
        _y = y;
        _renderer = GetComponent<Renderer>();
        _canvasManager = FindFirstObjectByType<CanvasManager>();
    }

    public void SetData(GridData data)
    {
        _id = data.id;
        _x = data.grid_x;
        _y = data.grid_y;
        _latSec = data.lat_sec;
        _lonSec = data.lon_sec;

        _idOfOwner = data.owner_of_the_grid_id ?? 0;
        _ownerNickname = data.owner_of_the_grid_nickname ?? "For Sale";

        MaterialPotentialWood = data.material_potential_wood;
        MaterialActualWood = data.material_actual_wood;
        MaterialWoodIsMined = data.material_wood_is_mined;
        MaterialWoodMiningValue = data.material_wood_mining_value;

        MaterialPotentialStone = data.material_potential_stone;
        MaterialActualStone = data.material_actual_stone;
        MaterialStoneIsMined = data.material_stone_is_mined;
        MaterialStoneMiningValue = data.material_stone_mining_value;

        MaterialPotentialGold = data.material_potential_gold;
        MaterialActualGold = data.material_actual_gold;
        MaterialGoldIsMined = data.material_gold_is_mined;
        MaterialGoldMiningValue = data.material_gold_mining_value;

        MaterialPotentialCopper = data.material_potential_copper;
        MaterialActualCopper = data.material_actual_copper;
        MaterialCopperIsMined = data.material_copper_is_mined;
        MaterialCopperMiningValue = data.material_copper_mining_value;

        MaterialPotentialCoal = data.material_potential_coal;
        MaterialActualCoal = data.material_actual_coal;
        MaterialCoalIsMined = data.material_coal_is_mined;
        MaterialCoalMiningValue = data.material_coal_mining_value;

        MaterialPotentialOil = data.material_potential_oil;
        MaterialActualOil = data.material_actual_oil;
        MaterialOilIsMined = data.material_oil_is_mined;
        MaterialOilMiningValue = data.material_oil_mining_value;

        MaterialPotentialUranium = data.material_potential_uranium;
        MaterialActualUranium = data.material_actual_uranium;
        MaterialUraniumIsMined = data.material_uranium_is_mined;
        MaterialUraniumMiningValue = data.material_uranium_mining_value;

        MaterialPotentialFood = data.material_potential_food;
        MaterialActualFood = data.material_actual_food;
        MaterialFoodIsMined = data.material_food_is_mined;
        MaterialFoodMiningValue = data.material_food_mining_value;

        MaterialPotentialWater = data.material_potential_water;
        MaterialActualWater = data.material_actual_water;
        MaterialWaterIsMined = data.material_water_is_mined;
        MaterialWaterMiningValue = data.material_water_mining_value;

        MaterialPotentialIron = data.material_potential_iron;
        MaterialActualIron = data.material_actual_iron;
        MaterialIronIsMined = data.material_iron_is_mined;
        MaterialIronMiningValue = data.material_iron_mining_value;

        SetColor(PlayerPrefs.GetInt("UserId", -1) == _idOfOwner ? Color.green : new Color(1f, 0.65f, 0f));
    }



    private void SetColor(Color color)
    {
        if (_renderer == null) _renderer = GetComponent<Renderer>();
        if (_renderer != null) _renderer.material.color = color;
    }

    /*private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                GridCell gridCell = hit.collider.GetComponent<GridCell>();
                if (gridCell != null && _canvasManager != null)
                {
                    Debug.Log($"📌 Click detected on: {gridCell.gameObject.name}");
                    _canvasManager.Show(gridCell);
                }
            }
        }
    }*/
    /*private void Update()
{
    if (Input.GetMouseButtonDown(0) && !_clickInProgress)
    {
        _clickInProgress = true;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            GridCell gridCell = hit.collider.GetComponent<GridCell>();
            if (gridCell != null && _canvasManager != null)
            {
                Debug.Log($"📌 Click detected on: {gridCell.gameObject.name}");
                _canvasManager.Show(gridCell);
            }
        }
    }

    // Reset the click flag when mouse button is released
    if (Input.GetMouseButtonUp(0))
    {
        _clickInProgress = false;
    }
}*//*
private void Update()
{
    Debug.Log($"Update called from: {gameObject.name}");

    if (Input.GetMouseButtonDown(0))
    {
        Debug.Log("MOUSE DOWN DETECTED");

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            GridCell gridCell = hit.collider.GetComponent<GridCell>();
            if (gridCell != null && _canvasManager != null)
            {
                Debug.Log($"📌 Click detected on: {gridCell.gameObject.name}");
                _canvasManager.Show(gridCell);
            }
        }
    }
}*/


    public void DebugPrint()
    {
        Debug.Log($"🟦 GRID CELL [{_x}, {_y}]  |  Owner ID: {_idOfOwner} | Owner Nickname: {_ownerNickname}");
        Debug.Log($"🌍 Coordinates: lat_sec = {_latSec}, lon_sec = {_lonSec}");

        Debug.Log($"🌲 Wood -> Potential: {MaterialPotentialWood}, Actual: {MaterialActualWood}, Is Mined: {MaterialWoodIsMined}, Mining Value: {MaterialWoodMiningValue}");
        Debug.Log($"🪨 Stone -> Potential: {MaterialPotentialStone}, Actual: {MaterialActualStone}, Is Mined: {MaterialStoneIsMined}, Mining Value: {MaterialStoneMiningValue}");
        Debug.Log($"🥇 Gold -> Potential: {MaterialPotentialGold}, Actual: {MaterialActualGold}, Is Mined: {MaterialGoldIsMined}, Mining Value: {MaterialGoldMiningValue}");
        Debug.Log($"🟠 Copper -> Potential: {MaterialPotentialCopper}, Actual: {MaterialActualCopper}, Is Mined: {MaterialCopperIsMined}, Mining Value: {MaterialCopperMiningValue}");
        Debug.Log($"🪵 Coal -> Potential: {MaterialPotentialCoal}, Actual: {MaterialActualCoal}, Is Mined: {MaterialCoalIsMined}, Mining Value: {MaterialCoalMiningValue}");
        Debug.Log($"🛢 Oil -> Potential: {MaterialPotentialOil}, Actual: {MaterialActualOil}, Is Mined: {MaterialOilIsMined}, Mining Value: {MaterialOilMiningValue}");
        Debug.Log($"☢ Uranium -> Potential: {MaterialPotentialUranium}, Actual: {MaterialActualUranium}, Is Mined: {MaterialUraniumIsMined}, Mining Value: {MaterialUraniumMiningValue}");
        Debug.Log($"🍎 Food -> Potential: {MaterialPotentialFood}, Actual: {MaterialActualFood}, Is Mined: {MaterialFoodIsMined}, Mining Value: {MaterialFoodMiningValue}");
        Debug.Log($"💧 Water -> Potential: {MaterialPotentialWater}, Actual: {MaterialActualWater}, Is Mined: {MaterialWaterIsMined}, Mining Value: {MaterialWaterMiningValue}");
        Debug.Log($"🛠 Iron -> Potential: {MaterialPotentialIron}, Actual: {MaterialActualIron}, Is Mined: {MaterialIronIsMined}, Mining Value: {MaterialIronMiningValue}");
    }

    public int GetGridId()
    {
        return _id;
    }

    public int GerGridOwnerId()
    {
        return _idOfOwner;
    }
}
