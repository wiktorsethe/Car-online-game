<?php
include('db_con.php');
$userId = $_POST["userId"];

$deleteQuery = "DELETE FROM active_sessions WHERE user_id = ".$userId." ";
if ($conn->query($deleteQuery) === TRUE) {
    echo 'Deleted';
} else {
    echo "Error: " . $deleteQuery . "<br>" . $conn->error;
}
?>