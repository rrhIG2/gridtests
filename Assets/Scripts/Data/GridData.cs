[System.Serializable]
public class GridData
{
    // Basic grid information
    public int id;
    public int grid_x;
    public int grid_y;
    public int? lat_sec;
    public int? lon_sec;

    // Ownership
    public int? owner_of_the_grid_id;
    public string owner_of_the_grid_nickname;

    // Material potentials and actual amounts
    public float? material_potential_wood;
    public float? material_actual_wood;
    public bool? material_wood_is_mined;
    public float material_wood_mining_value;

    public float? material_potential_stone;
    public float? material_actual_stone;
    public bool? material_stone_is_mined;
    public float material_stone_mining_value;

    public float? material_potential_gold;
    public float? material_actual_gold;
    public bool? material_gold_is_mined;
    public float material_gold_mining_value;

    public float? material_potential_copper;
    public float? material_actual_copper;
    public bool? material_copper_is_mined;
    public float material_copper_mining_value;

    public float? material_potential_coal;
    public float? material_actual_coal;
    public bool? material_coal_is_mined;
    public float material_coal_mining_value;

    public float? material_potential_oil;
    public float? material_actual_oil;
    public bool? material_oil_is_mined;
    public float material_oil_mining_value;

    public float? material_potential_uranium;
    public float? material_actual_uranium;
    public bool? material_uranium_is_mined;
    public float material_uranium_mining_value;

    public float? material_potential_food;
    public float? material_actual_food;
    public bool? material_food_is_mined;
    public float material_food_mining_value;

    public float? material_potential_water;
    public float? material_actual_water;
    public bool? material_water_is_mined;
    public float material_water_mining_value;

    public float? material_potential_iron;
    public float? material_actual_iron;
    public bool? material_iron_is_mined;
    public float material_iron_mining_value;

    // Mining and production
    public float? material_mining;
    public string production_type;

    // Timestamps
    public string created_at;
    public string updated_at;
}
