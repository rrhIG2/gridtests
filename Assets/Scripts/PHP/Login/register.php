<?php
include 'db_config.php';

header("Content-Type: application/json");

// Get JSON data from Unity
$rawData = file_get_contents('php://input');
$data = json_decode($rawData, true);

// Debugging output
if (!$data) {
    echo json_encode([
        "message" => "❌ JSON data is null or not parsed correctly.",
        "raw_input" => $rawData,
        "json_last_error" => json_last_error_msg()
    ]);
    exit();
}

$nickname = $data['nickname'] ?? null;
$password = $data['password'] ?? null;

if (empty($nickname) || empty($password)) {
    echo json_encode(["message" => "❌ Nickname or password is empty."]);
    exit();
}

// Check if user already exists
$sql = "SELECT COUNT(*) AS count FROM users WHERE nickname=?";
$stmt = $conn->prepare($sql);
$stmt->bind_param("s", $nickname);
$stmt->execute();
$result = $stmt->get_result();
$row = $result->fetch_assoc();

if ($row['count'] > 0) {
    echo json_encode(["message" => "Nickname is already taken."]);
    $stmt->close();
    $conn->close();
    exit();
}

// Register new user
$sql = "INSERT INTO users (nickname, password) VALUES (?, ?)";
$stmt = $conn->prepare($sql);
$stmt->bind_param("ss", $nickname, $password);

if ($stmt->execute()) {
    echo json_encode(["message" => "Registration successful."]);
} else {
    echo json_encode(["message" => "Registration failed."]);
}

$stmt->close();
$conn->close();
?>
