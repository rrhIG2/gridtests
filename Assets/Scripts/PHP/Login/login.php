<?php
include 'db_config.php';

// Get JSON data from Unity
$data = json_decode(file_get_contents('php://input'), true);

$nickname = $data['nickname'] ?? null;
$password = $data['password'] ?? null;

if (empty($nickname) || empty($password)) {
    echo json_encode(["message" => "❌ Nickname or password is empty."]);
    exit();
}

// Check if the user exists
$sql = "SELECT id, nickname FROM users WHERE nickname=? AND password=?";
$stmt = $conn->prepare($sql);
$stmt->bind_param("ss", $nickname, $password);
$stmt->execute();
$result = $stmt->get_result();
$row = $result->fetch_assoc();

if ($row) {
    echo json_encode([
        "message" => "Login successful.",
        "userId" => $row['id'],
        "nickname" => $row['nickname']
    ]);
} else {
    echo json_encode(["message" => "Invalid nickname or password."]);
}

$stmt->close();
$conn->close();
?>
