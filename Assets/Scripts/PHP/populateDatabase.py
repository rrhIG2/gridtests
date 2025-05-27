import random
import mysql.connector
from datetime import datetime

# Database configuration
DB_CONFIG = {
    'host': 'dbadmin.ukfig2.sk',
    'port': '3306',
    'user': 'arcGis',
    'password': 'arcGis123_',
    'database': 'arcGis'
}

# Material types
materials = [
    'wood', 'stone', 'iron', 'gold', 'copper',
    'coal', 'oil', 'uranium', 'food', 'water'
]

# Generate coordinates (-100 to 100 for x and y)
grid_coordinates = [(x, y) for x in range(-100, 101) for y in range(-100, 101)]

# Function to generate potential and actual material values
def generate_material_data():
    data = {}
    for material in materials:
        potential = random.randint(5000, 100000)
        actual = round(potential * random.randint(5, 20) * 0.1, 2)
        data[f'material_potential_{material}'] = potential
        data[f'material_actual_{material}'] = actual
        data[f'material_{material}_is_mined'] = 0  # always 0
    return data

# Connect to the database
conn = mysql.connector.connect(**DB_CONFIG)
cursor = conn.cursor()

# Prepare column list
column_parts = ['grid_x', 'grid_y', 'lat_sec', 'lon_sec', 'created_at', 'updated_at']
for material in materials:
    column_parts += [
        f'material_potential_{material}',
        f'material_actual_{material}',
        f'material_{material}_is_mined'
    ]

columns = ', '.join(column_parts)
placeholders = ', '.join(['%s'] * len(column_parts))
sql = f"INSERT INTO grids ({columns}) VALUES ({placeholders})"

# Insert in batches
batch_size = 500
batch_data = []
now = datetime.now().strftime('%Y-%m-%d %H:%M:%S')

for idx, (x, y) in enumerate(grid_coordinates):
    material_data = generate_material_data()

    values = [x, y, None, None, now, now]  # lat_sec, lon_sec = NULL
    for material in materials:
        values.append(material_data[f'material_potential_{material}'])
        values.append(material_data[f'material_actual_{material}'])
        values.append(material_data[f'material_{material}_is_mined'])

    batch_data.append(values)

    if len(batch_data) == batch_size:
        cursor.executemany(sql, batch_data)
        conn.commit()
        print(f"Inserted {idx + 1} / {len(grid_coordinates)} grids")
        batch_data.clear()

# Insert remaining
if batch_data:
    cursor.executemany(sql, batch_data)
    conn.commit()
    print(f"Inserted final {len(batch_data)} rows")

cursor.close()
conn.close()

print("✅ Grid population completed without lat/lon, mining, or production type.")
