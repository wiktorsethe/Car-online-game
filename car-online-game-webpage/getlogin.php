<?php
include('db_con.php');

$username = $_POST["username"];
$password = $_POST["password"];

$sql = "SELECT password, username, id FROM `Player` WHERE username = '".$username."' ";
$result = $conn->query($sql);

if ($result->num_rows > 0) {
    $row = $result->fetch_assoc();
    if ($row['password'] == $password) {
        // Przekazanie ID użytkownika do zarządzania sesją
        //include('session_manager.php');
        //manageSession($conn, $row['id']);
        echo $row['id']."|".$row['username'];
    } else {
        echo "Niepoprawne hasło.";
    }
} else {
    echo "Użytkownik nie istnieje.";
}
?>
