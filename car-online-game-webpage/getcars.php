<?php
include('db_con.php');

$playerId = $_POST["userid"];
$sql = "SELECT Pc.player_id, Pc.car_id, c.producer, c.model FROM Car c JOIN Player_cars Pc on c.id = Pc.car_id WHERE player_id='".$playerId."' ";
$result = $conn->query($sql);
if($result->num_rows > 0){
    while($row = $result->fetch_assoc()){
        echo $row["car_id"]."|".$row["producer"]."|".$row["model"]."\n";
    }
}