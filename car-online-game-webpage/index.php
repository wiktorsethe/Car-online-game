<!DOCTYPE html>
<html lang="pl">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Cars Online Game - Pobierz Grę</title>
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
            position: relative;
        }

        h1 {
            margin-bottom: 20px;
            font-size: 2.5em;
        }

        .download-btn {
            display: inline-block;
            background-color: #007bff;
            color: white;
            padding: 15px 30px;
            text-decoration: none;
            font-size: 1.2em;
            border-radius: 5px;
            transition: background-color 0.3s ease;
        }

        .download-btn:hover {
            background-color: #0056b3;
        }

        .side-link {
            position: absolute;
            top: 10px;
            right: 10px;
            font-size: 1em;
        }

        .side-link a {
            text-decoration: none;
            color: #007bff;
        }

        .side-link a:hover {
            color: #0056b3;
        }

    </style>
</head>
<body>
<div class="container">
    <h1>Cars Online Game</h1>
    <!-- Link do pobrania gry -->
    <a href="/build.zip" download class="download-btn">Pobierz grę</a>

    <!-- Link do rejestracji po prawej stronie -->
    <div class="side-link">
        <a href="register.php">Stwórz konto</a>
    </div>
</div>
</body>
</html>
