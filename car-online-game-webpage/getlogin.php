<?php
include('db_con.php');

$username = $_POST["username"];
$password = $_POST["password"];

$sql = "SELECT password, id FROM `Player` WHERE username = '".$username."' ";
$result = $conn->query($sql);

if($result->num_rows > 0) {
    while($row = $result->fetch_assoc()) {
        if($row['password'] == $password){
            $userId = $row['id'];

            $query = "SELECT * FROM active_sessions WHERE user_id = ".$userId." ";
            $stmt = $conn->prepare($query);
            $stmt->execute();
            $result2 = $stmt->get_result();
            if ($result2->num_rows > 0) {
                // User already logged in
                echo "You are already logged in elsewhere.";
                exit;
            } else {
                try {
                    $sessionToken = bin2hex(random_bytes(16));
                    echo $sessionToken;
                } catch (\Random\RandomException $e) {
                    echo 'catch';
                }

                $insertQuery = "INSERT INTO active_sessions (user_id, session_token) VALUES ($userId, '".$sessionToken."')";
                if ($conn->query($insertQuery) === TRUE) {
                    echo $userId."|".$username."|".$password;
                } else {
                    echo "Error: " . $insertQuery . "<br>" . $conn->error;
                }
            }
        } else {
            echo "password incorrect";
        }
    }
}
else {
    echo "username incorrect";
}
?>