<?php
$host = '54.38.52.204';
$username = 'ubuntu';
$password = '27rhN@Vj^Exf*e';
$database = 'eoDB';

if (!$conn = mysqli_connect($host, $username, $password, $database)) {
    die("Błąd połączenia: " . mysqli_connect_error());
}
?>