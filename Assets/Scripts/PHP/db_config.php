<?php
$host = 'dbadmin.ukfig2.sk';
$db = 'arcGis';
$user = 'arcGis';
$pass = 'arcGis123_';

// Create connection
$conn = new mysqli($host, $user, $pass, $db);

// Check connection
if ($conn->connect_error) {
    die("Connection failed: " . $conn->connect_error);
}
?>
