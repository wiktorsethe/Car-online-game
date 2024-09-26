using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{
    public float followSpeed = 5f; // Szybkość podążania za graczem
    private Transform playerTransform;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        // Wyszukaj gracza po uruchomieniu sceny
        FindPlayer();
    }

    private void OnEnable()
    {
        // Subskrybujemy event wczytywania sceny
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // Odsubskrybowanie eventu
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Wywoływane po załadowaniu każdej nowej sceny
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindPlayer();
    }

    // Wyszukiwanie gracza na scenie
    private void FindPlayer()
    {
        GameObject player = GameObject.FindWithTag("Player");

        if (player != null)
        {
            playerTransform = player.transform;
            Debug.Log("Znaleziono gracza. Rozpoczynam podążanie.");
        }
        else
        {
            Debug.LogWarning("Nie znaleziono obiektu z tagiem 'Player'.");
        }
    }

    private void Update()
    {
        if (playerTransform != null)
        {
            // Podążanie za graczem z określoną prędkością
            FollowTarget();
        }
    }

    // Funkcja podążania za graczem
    private void FollowTarget()
    {
        // Aktualizacja pozycji, interpolując (smooth movement) w kierunku gracza
        transform.position = Vector2.Lerp(transform.position, playerTransform.position, followSpeed * Time.deltaTime);
    }
}
