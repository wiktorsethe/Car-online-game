<?php
include('db_con.php');

$nickname = $_POST["nickname"];
$gender = $_POST["gender"];
$userId = $_POST["userid"];

$checkSql = "SELECT * FROM `Character` WHERE character_name ='".$nickname."' ";
$result = $conn->query($checkSql);

if ($result->num_rows > 0) {
    echo "Nickname is already taken";
} else {
    $sql = "INSERT INTO `Character` (character_name, gender, Player_id) VALUES ('".$nickname."', '".$gender."', '".$userId."')";
    if ($conn->query($sql) === TRUE) {
        echo true;
    } else {
        echo false;
    }
}