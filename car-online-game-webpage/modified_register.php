<?php
// Załączenie pliku db_con.php
include('db_con.php');

// Funkcja do przekierowania na inną stronę
function redirectTo($url) {
    header('Location: ' . $url);
    exit();
}

// Sprawdzenie, czy dane zostały przesłane z formularza
if ($_SERVER["REQUEST_METHOD"] == "POST") {
    // Przechwycenie danych z formularza
    $new_username = $_POST['new_username'];
    $new_password = $_POST['new_password'];

    // Zabezpieczenie przed atakami typu SQL Injection
    $new_username = mysqli_real_escape_string($conn, $new_username);
    $new_password = mysqli_real_escape_string($conn, $new_password);

    // Zapytanie SQL dodające nowego użytkownika do bazy danych
    $query = "INSERT INTO Player (username, password) VALUES ('$new_username', '$new_password')";

    // Wykonanie zapytania
    if ($conn->query($query) === TRUE) {
        // Nowy użytkownik został dodany pomyślnie
        
        // Get the last inserted player_id (the newly registered user)
        $last_player_id = $conn->insert_id;

        // Insert into Player_cars with player_id and car_id = 1
        $car_insert_query = "INSERT INTO Player_cars (player_id, car_id) VALUES ($last_player_id, 1)";

        // Check if the car insertion was successful
        if ($conn->query($car_insert_query) === TRUE) {
            // Car associated successfully
            redirectTo('index.php'); // Redirect to the desired page after successful registration and car assignment
        } else {
            // Handle the error if car association failed
            echo "Error: " . $car_insert_query . "<br>" . $conn->error;
        }
     // Przekierowanie na stronę eldritchodyssey.com/game
    } else {
        // Błąd podczas dodawania nowego użytkownika
        echo "Błąd: " . $query . "<br>" . $conn->error;
    }
}
?>

<!DOCTYPE html>
<html lang="pl">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Cars Online Game - Rejestracja</title>
    <style>
        /* Resetowanie stylów domyślnych */
        * {
            margin: 0;
            padding: 0;
            box-sizing: border-box;
        }

        body {
            display: flex;
            justify-content: center;
            align-items: center;
            height: 100vh;
            background-color: #f0f0f0;
            font-family: Arial, sans-serif;
        }

        .container {
            text-align: center;
            background-color: white;
            padding: 40px;
            box-shadow: 0 0 20px rgba(0, 0, 0, 0.1);
            border-radius: 10px;
            width: 100%;
            max-width: 400px;
        }

        h2 {
            margin-bottom: 20px;
            font-size: 2em;
        }

        form {
            display: flex;
            flex-direction: column;
            align-items: center;
        }

        label {
            margin: 10px 0 5px 0;
            font-size: 1em;
        }

        input[type="text"],
        input[type="password"] {
            width: 100%;
            padding: 10px;
            margin-bottom: 15px;
            border: 1px solid #ccc;
            border-radius: 5px;
            font-size: 1em;
        }

        input[type="submit"] {
            padding: 10px 20px;
            background-color: #007bff;
            color: white;
            border: none;
            border-radius: 5px;
            font-size: 1.2em;
            cursor: pointer;
            transition: background-color 0.3s ease;
        }

        input[type="submit"]:hover {
            background-color: #0056b3;
        }

        .back-link {
            margin-top: 20px;
            font-size: 1em;
        }

        .back-link a {
            color: #007bff;
            text-decoration: none;
        }

        .back-link a:hover {
            color: #0056b3;
        }
    </style>
</head>
<body>
<div class="container">
    <h2>Formularz Rejestracji</h2>

    <!-- Formularz rejestracyjny -->
    <form action="" method="post">
        <label for="new_username">Nowa nazwa użytkownika:</label>
        <input type="text" id="new_username" name="new_username" required>

        <label for="new_password">Nowe hasło:</label>
        <input type="password" id="new_password" name="new_password" required>

        <input type="submit" value="Zarejestruj">
    </form>

    <!-- Link powrotu do strony głównej -->
    <div class="back-link">
        <a href="index.php">Powrót do strony głównej</a>
    </div>
</div>
</body>
</html>
