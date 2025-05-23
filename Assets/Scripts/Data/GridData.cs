[System.Serializable]
public class GridData
{
    // Basic grid information
    public int id;
    public int grid_x;
    public int grid_y;

    // Ownership information
    public int? ownerOfTheGridId;
    public string ownerOfTheGridNickname;

    // Material potentials and actual amounts
    public double material_potential_wood;
    public double material_actual_wood;
    
    public double material_potential_stone;
    public double material_actual_stone;
    
    public double material_potential_iron;
    public double material_actual_iron;
    
    public double material_potential_gold;
    public double material_actual_gold;
    
    public double material_potential_copper;
    public double material_actual_copper;
    
    public double material_potential_coal;
    public double material_actual_coal;
    
    public double material_potential_oil;
    public double material_actual_oil;
    
    public double material_potential_uranium;
    public double material_actual_uranium;
    
    public double material_potential_food;
    public double material_actual_food;
    
    public double material_potential_water;
    public double material_actual_water;

    // Mining and production
    public double? material_mining;
    public string production_type;
}
