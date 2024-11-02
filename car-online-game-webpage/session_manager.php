<?php
include('db_con.php');

$userId = $_POST["userId"]; // Pobranie userId z żądania

// Sprawdzenie, czy userId jest ustawione
if (isset($userId)) {
    try {
        $sessionToken = bin2hex(random_bytes(16)); // Generowanie tokena sesji

        // Wstawienie sesji do bazy danych
        $insertQuery = "INSERT INTO active_sessions (user_id, session_token) VALUES ($userId, '".$sessionToken."')";

        if ($conn->query($insertQuery) === TRUE) {
            // Zwrócenie odpowiedzi z tokenem
            echo "Sesja utworzona: token=" . $sessionToken;
        } else {
            echo "Błąd podczas tworzenia sesji.";
        }
    } catch (Exception $e) {
        echo 'Wystąpił błąd podczas generowania tokena.';
    }
} else {
    echo "Brak wymaganego parametru userId.";
}
?>
