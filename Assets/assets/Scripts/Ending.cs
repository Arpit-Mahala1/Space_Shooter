using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class Ending : MonoBehaviour
{
    [Header("Timer Settings")]
    public float gameTime = 300f; // Game time in seconds (5 minutes)
    private float timeRemaining;

    [Header("End Settings")]
    public GameObject endPrefab;  // The "End" prefab where the game ends
    public Camera mainCamera;     // Main camera to be disabled
    public GameObject uiPanel;    // UI panel to display stats
    public Text winnerText;       // Text to show the winner's stats

    private bool gameEnded = false;
    private GameObject player1;
    private GameObject player2;
    private GameObject winner;
    private Camera winnerCamera;

    void Start()
    {
        // Initialize the timer
        timeRemaining = gameTime;

        // Hide the UI at the start of the game
        uiPanel.SetActive(false);

        // Find players by their tags (assuming they are instantiated and tagged properly)
        StartCoroutine(FindPlayers());
    }

    void Update()
    {
        if (!gameEnded)
        {
            // Countdown timer logic
            timeRemaining -= Time.deltaTime;

            if (timeRemaining <= 0f)
            {
                EndGame(null); // If timer runs out, end the game (no winner)
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // If either player collides with the end prefab
        if (!gameEnded)
        {
            if (other.CompareTag("Player1"))
            {
                EndGame(player1); // Player1 wins
            }
            else if (other.CompareTag("Player2"))
            {
                EndGame(player2); // Player2 wins
            }
        }
    }

    IEnumerator FindPlayers()
    {
        // Wait for a short time to ensure players are spawned
        yield return new WaitForSeconds(1f);

        // Find players by their tags
        player1 = GameObject.FindGameObjectWithTag("Player1");
        player2 = GameObject.FindGameObjectWithTag("Player2");

        // Check if players are found
        if (player1 == null || player2 == null)
        {
            Debug.LogError("Players not found! Ensure they are tagged as 'Player1' and 'Player2'.");
        }
    }

    void EndGame(GameObject winnerPlayer)
    {
        gameEnded = true;

        // Disable all cameras
        mainCamera.gameObject.SetActive(false);

        // Disable all player scripts
        if (player1 != null)
            DisablePlayer(player1);
        if (player2 != null)
            DisablePlayer(player2);

        // Handle the winner logic
        if (winnerPlayer != null)
        {
            winner = winnerPlayer;
            winnerCamera = winner.GetComponentInChildren<Camera>();

            // Activate the winner's camera
            if (winnerCamera != null)
            {
                winnerCamera.gameObject.SetActive(true);
            }
        }

        // Show UI with winner's stats
        ShowStats(winner);
    }

    void DisablePlayer(GameObject player)
    {
        // Disable all scripts on the player
        MonoBehaviour[] scripts = player.GetComponentsInChildren<MonoBehaviour>();
        foreach (var script in scripts)
        {
            script.enabled = false;
        }

        // Disable the player's camera
        Camera playerCamera = player.GetComponentInChildren<Camera>();
        if (playerCamera != null)
        {
            playerCamera.gameObject.SetActive(false);
        }
    }

    void ShowStats(GameObject winner)
    {
        uiPanel.SetActive(true); // Show the UI panel
        if (winner != null)
        {
            winnerText.text = $"{winner.name} Wins!\nTime Left: {timeRemaining:F2} seconds"; // Example stats
        }
        else
        {
            winnerText.text = "Time's Up! No Winner!";
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload the current scene to restart the game
    }
}
