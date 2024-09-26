<?php
include('db_con.php');

$playerId = $_POST["userid"];
//$sql1 = "SELECT * FROM `Player_cars` WHERE player_id='".$playerId."' ";
$sql = "SELECT Pc.player_id, c.producer, c.model FROM Car c JOIN Player_cars Pc on c.id = Pc.car_id WHERE player_id='".$playerId."' ";
$result = $conn->query($sql);
if($result->num_rows > 0){
    while($row = $result->fetch_assoc()){
        echo $row["producer"]."|".$row["model"]."\n";
    }
}