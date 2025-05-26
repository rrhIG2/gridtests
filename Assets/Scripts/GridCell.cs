using UnityEngine;
using TMPro;
using System;

public class GridCell : MonoBehaviour
{
    [Header("Grid Coordinates")]
    [SerializeField] private int _x;
    [SerializeField] private int _y;

    [Header("Grid Data")]
    [SerializeField] private int _idOfOwner;
    [SerializeField] private string _ownerNickname;

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
    [SerializeField] private double _materialMining;
    [SerializeField] private MaterialType _productionType;

    [Header("Label Settings")]
    private TextMeshPro _label;
    private Renderer _renderer;

    private GridInfoUI _gridInfoUI;

    public int X => _x;
    public int Y => _y;
    public int IdOfOwner => _idOfOwner;
    public string OwnerNickname => _ownerNickname;
    public double MaterialMining => _materialMining;
    public MaterialType ProductionType => _productionType;

    public void Initialize(int x, int y)
    {
        _x = x;
        _y = y;
        _renderer = GetComponent<Renderer>();
        _gridInfoUI = FindObjectOfType<GridInfoUI>();
    }

    public void SetData(GridData data)
    {
        _idOfOwner = data.ownerOfTheGridId ?? 0;
        _ownerNickname = data.ownerOfTheGridNickname;

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

        _materialMining = data.material_mining ?? 0;

        if (Enum.TryParse(data.production_type, true, out MaterialType parsedType))
        {
            _productionType = parsedType;
        }
        else
        {
            Debug.LogWarning($"⚠️ Could not parse production type: {data.production_type}. Defaulting to None.");
            _productionType = MaterialType.None;
        }

        SetColor(PlayerPrefs.GetInt("UserId", -1) == _idOfOwner ? Color.green : new Color(1f, 0.65f, 0f));
    }

    private void SetColor(Color color)
    {
        if (_renderer == null) _renderer = GetComponent<Renderer>();
        if (_renderer != null) _renderer.material.color = color;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                GridCell gridCell = hit.collider.GetComponent<GridCell>();
                if (gridCell != null && _gridInfoUI != null)
                {
                    Debug.Log($"📌 Click detected on: {gridCell.gameObject.name}");
                    _gridInfoUI.Show(gridCell);
                }
            }
        }
    }
}
