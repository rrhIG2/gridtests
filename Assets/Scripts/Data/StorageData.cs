[System.Serializable]
public class StorageData
{
    // Storage identity
    public int id;

    // Owner user ID (foreign key to users.id)
    public int storage_owner;

    // Currency
    public float geoCoin;

    // Resource: Wood
    public float? storage_capacity_wood;
    public float? storage_actual_wood;

    // Resource: Stone
    public float? storage_capacity_stone;
    public float? storage_actual_stone;

    // Resource: Gold
    public float? storage_capacity_gold;
    public float? storage_actual_gold;

    // Resource: Copper
    public float? storage_capacity_copper;
    public float? storage_actual_copper;

    // Resource: Coal
    public float? storage_capacity_coal;
    public float? storage_actual_coal;

    // Resource: Oil
    public float? storage_capacity_oil;
    public float? storage_actual_oil;

    // Resource: Uranium
    public float? storage_capacity_uranium;
    public float? storage_actual_uranium;

    // Resource: Food
    public float? storage_capacity_food;
    public float? storage_actual_food;

    // Resource: Water
    public float? storage_capacity_water;
    public float? storage_actual_water;

    // Timestamps
    public string created_at;
    public string updated_at;
}
