import random
import mysql.connector
from datetime import datetime

# Database configuration (to be filled with user-provided credentials)
DB_CONFIG = {
    'host': 'dbadmin.ukfig2.sk',
    'port': '3306',
    'user': 'arcGis',
    'password': 'arcGis123_',
    'database': 'arcGis'
}

# Material list
materials = [
    'wood', 'stone', 'iron', 'gold', 'copper',
    'coal', 'oil', 'uranium', 'food', 'water'
]

# Generate the grid coordinates for -100 to 100 on both axes (40000 total grids)
grid_coordinates = [(x, y) for x in range(-100, 101) for y in range(-100, 101)]

# Function to generate potential and actual values
def generate_material_data():
    data = {}
    for material in materials:
        potential = random.randint(5000, 100000)
        actual_percentage = random.randint(5, 20) * 0.1
        actual = round(potential * actual_percentage, 2)
        data[f"material_potential_{material}"] = potential
        data[f"material_actual_{material}"] = actual
    return data

# Connect to database
conn = mysql.connector.connect(**DB_CONFIG)
cursor = conn.cursor()

# SQL Insert Template
sql = """
INSERT INTO grids (grid_x, grid_y, created_at, updated_at, 
{columns}, material_mining, production_type) 
VALUES (%s, %s, %s, %s, {placeholders}, %s, %s)
"""

# Prepare columns and placeholders
columns = ', '.join([f"material_potential_{m}, material_actual_{m}" for m in materials])
placeholders = ', '.join(['%s'] * (len(materials) * 2))

# Batch insert
batch_size = 500
batch_data = []
current_time = datetime.now().strftime('%Y-%m-%d %H:%M:%S')

for idx, (x, y) in enumerate(grid_coordinates):
    material_data = generate_material_data()
    values = [x, y, current_time, current_time] + list(material_data.values()) + [0, 'None']
    batch_data.append(values)

    # When batch is full, execute insert
    if len(batch_data) == batch_size:
        cursor.executemany(sql.format(columns=columns, placeholders=placeholders), batch_data)
        conn.commit()
        batch_data.clear()
        print(f"Inserted {idx + 1} / {len(grid_coordinates)} grids.")

# Insert remaining data if any
if batch_data:
    cursor.executemany(sql.format(columns=columns, placeholders=placeholders), batch_data)
    conn.commit()

# Close connections
cursor.close()
conn.close()

# Output message
output_message = "✅ Database population completed successfully."
output_message
