
<?php
include('db_con.php');

// Sprawdzenie połączenia
if ($conn->connect_error) {
    die("Błąd połączenia: " . $conn->connect_error);
}

// Funkcja do obsługi przesyłania pliku
if ($_SERVER['REQUEST_METHOD'] == 'POST' && isset($_FILES['file'])) {
    $image = $_FILES['file']['tmp_name'];  // Ścieżka do przesłanego pliku na serwerze
    $imageName = $_FILES['file']['name'];  // Nazwa pliku
    $imageSize = $_FILES['file']['size'];  // Rozmiar pliku
    $imageType = $_FILES['file']['type'];  // Typ MIME pliku

    // Sprawdzanie, czy plik to obraz (opcjonalne)
    $allowedTypes = ['image/jpeg', 'image/png', 'image/gif'];
    if (!in_array($imageType, $allowedTypes)) {
        echo "Nieprawidłowy format pliku. Dozwolone formaty: JPG, PNG, GIF.";
        exit;
    }

    // Odczyt danych binarnych obrazu
    $imageData = file_get_contents($image);

    // Zapisanie obrazu do bazy danych
    $stmt = $conn->prepare("INSERT INTO images (image_name, image_data) VALUES (?, ?)");
    $stmt->bind_param("sb", $imageName, $imageData);

    // Obsługa danych binarnych większych niż 1 MB
    $stmt->send_long_data(1, $imageData);

    if ($stmt->execute()) {
        echo "Obraz został pomyślnie zapisany w bazie danych.";
    } else {
        echo "Błąd podczas zapisywania obrazu: " . $conn->error;
    }

    $stmt->close();
}

// Zamknięcie połączenia
$conn->close();
?>

<!-- Formularz HTML do przesyłania pliku -->
<!DOCTYPE html>
<html lang="pl">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Prześlij obraz</title>
</head>
<body>
<h1>Prześlij obraz do bazy danych</h1>
<form action="" method="post" enctype="multipart/form-data">
    <label for="file">Wybierz obraz:</label>
    <input type="file" name="file" id="file" required>
    <br><br>
    <input type="submit" value="Wgraj obraz">
</form>
</body>
</html>
