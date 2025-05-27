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
    public float? MaterialPotentialWood { get; private set; }
    public float? MaterialActualWood { get; private set; }
    public float? MaterialPotentialStone { get; private set; }
    public float? MaterialActualStone { get; private set; }
    public float? MaterialPotentialGold { get; private set; }
    public float? MaterialActualGold { get; private set; }
    public float? MaterialPotentialCopper { get; private set; }
    public float? MaterialActualCopper { get; private set; }
    public float? MaterialPotentialCoal { get; private set; }
    public float? MaterialActualCoal { get; private set; }
    public float? MaterialPotentialOil { get; private set; }
    public float? MaterialActualOil { get; private set; }
    public float? MaterialPotentialUranium { get; private set; }
    public float? MaterialActualUranium { get; private set; }
    public float? MaterialPotentialFood { get; private set; }
    public float? MaterialActualFood { get; private set; }
    public float? MaterialPotentialWater { get; private set; }
    public float? MaterialActualWater { get; private set; }
    public float? MaterialPotentialIron { get; private set; }
    public float? MaterialActualIron { get; private set; }

    [Header("Label Settings")]
    private TextMeshPro _label;
    private Renderer _renderer;
    private GridInfoUI _gridInfoUI;

    public int X => _x;
    public int Y => _y;

    public void Initialize(int x, int y)
    {
        _x = x;
        _y = y;
        _renderer = GetComponent<Renderer>();
        _gridInfoUI = FindFirstObjectByType<GridInfoUI>();
    }

    public void SetData(GridData data)
    {
        _idOfOwner = data.owner_of_the_grid_id ?? 0;
        _ownerNickname = data.owner_of_the_grid_nickname ?? "For Sale";

        MaterialPotentialWood = data.material_potential_wood;
        MaterialActualWood = data.material_actual_wood;

        MaterialPotentialStone = data.material_potential_stone;
        MaterialActualStone = data.material_actual_stone;

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

        MaterialPotentialIron = data.material_potential_iron;
        MaterialActualIron = data.material_actual_iron;

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
