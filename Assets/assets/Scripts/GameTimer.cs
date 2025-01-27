//using UnityEngine;
//using TMPro;

//public class GameTimer : MonoBehaviour
//{
//    public float gameDuration = 90f; // 90 seconds
//    private float timer;
//    private float timer2;
//    private bool gameEnded = false;
//    private bool isGameActive = false;
//    private bool playersAssigned = false;

//    public GameObject player1; // Reference to Player 1
//    public GameObject player2; // Reference to Player 2
//    public UI_Manager uiManager;
//    public TextMeshProUGUI textMeshPro;

//    public void Initialize()
//    {
//        timer = 0f;
//        timer2 = 0f;  // Reset timer2 as well
//        gameEnded = false;  // Reset game ended state
//        isGameActive = true;
//        playersAssigned = false;
//    }

//    void Update()
//    {

//        if (player1 == null) player1 = GameObject.Find("Player1");
//        if (player2 == null) player2 = GameObject.Find("Player2");

//        if (gameEnded==true)
//        {
//            if (uiManager != null)
//            {
//                uiManager.OnGameEnd();
//            }
//            return;
//        }




//        // Check for end conditions
//        if (isGameActive)
//        {
//            // Decrease the timer
//            timer += Time.deltaTime;

//                timer2 += Time.deltaTime;


//            //Check for end conditions
//            if (timer >= gameDuration)
//                {
//                    EndGame();
//                }
//            else if (timer2 > 2f && (player1 == null || player2 == null))
//            {
//                // Handle player disconnection mid-game
//                Debug.LogWarning("A player has disconnected!");
//                EndGame();
//            }
//        }
//    }

//    private void LateUpdate()
//    {
//        int minutes = Mathf.FloorToInt(timer / 60);
//        int seconds = Mathf.FloorToInt(timer % 60);
//        textMeshPro.SetText(string.Format("{0:00}:{1:00}", minutes, seconds));
//    }
//    void EndGame()
//    {
//        gameEnded = true;
//        isGameActive = false;  // Stop the timer

//        // Determine the winner
//        string winner = "";
//        if (player1 == null && player2 != null)
//        {
//            winner = "Player 2 Wins!";
//        }
//        else if (player2 == null && player1 != null)
//        {
//            winner = "Player 1 Wins!";
//        }
//        else
//        {
//            winner = "Time's Up! It's a Draw!";
//        }

//        // Display the winner message
//        Debug.Log(winner);

//        // Trigger game end UI
//        if (uiManager != null)
//        {
//            uiManager.OnGameEnd();
//        }
//    }

//    //void RestartGame()
//    //{
//    //    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
//    //}
//}

//using UnityEngine;
//using TMPro;

//public class GameTimer : MonoBehaviour
//{
//    public float gameDuration = 90f; // 90 seconds
//    private float timer;
//    private float timer2;
//    private bool gameEnded = false;
//    private bool isGameActive = false;
//    public GameObject player1;
//    public GameObject player2;
//    public UI_Manager uiManager;
//    public TextMeshProUGUI textMeshPro;

//    public void Initialize()
//    {
//        timer = 0f;
//        timer2 = 0f;
//        gameEnded = false;
//        isGameActive = true;
//    }

//    void Update()
//    {
//        if (gameEnded) return;

//        if (player1 == null) player1 = GameObject.Find("Player1");
//        if (player2 == null) player2 = GameObject.Find("Player2");

//        if (isGameActive)
//        {
//            timer += Time.deltaTime;
//            timer2 += Time.deltaTime;

//            // Ensure TextMeshPro is updated in real-time
//            UpdateTimerDisplay();

//            if (timer >= gameDuration)
//            {
//                EndGame();
//            }
//            else if (timer2 > 2f && (player1 == null || player2 == null))
//            {
//                EndGame();
//            }
//        }
//    }

//    void UpdateTimerDisplay()
//    {
//        if (textMeshPro != null)
//        {
//            int minutes = Mathf.FloorToInt(timer / 60);
//            int seconds = Mathf.FloorToInt(timer % 60);
//            textMeshPro.SetText(string.Format("{0:00}:{1:00}", minutes, seconds));
//        }
//    }

//    void EndGame()
//    {
//        gameEnded = true;
//        isGameActive = false;

//        string winner = DetermineWinner();
//        Debug.Log(winner);

//        if (uiManager != null)
//        {
//            uiManager.OnGameEnd();
//        }
//    }

//    string DetermineWinner()
//    {
//        if (player1 == null && player2 != null)
//            return "Player 2 Wins!";
//        else if (player2 == null && player1 != null)
//            return "Player 1 Wins!";
//        else
//            return "Time's Up! It's a Draw!";
//    }
//}

//using UnityEngine;
//using TMPro;

//public class GameTimer : MonoBehaviour
//{
//    [SerializeField] private float gameDuration = 90f;
//    [SerializeField] private TextMeshProUGUI timerText;
//    [SerializeField] private UI_Manager uiManager;

//    private float timer;
//    private bool isGameActive;
//    private bool gameEnded;

//    private void Awake()
//    {
//        // Cache references early

//        ResetTimer();
//        if (timerText == null) timerText = GetComponentInChildren<TextMeshProUGUI>();
//        if (uiManager == null) uiManager = FindObjectOfType<UI_Manager>();
//    }
//    private void ResetTimer()
//    {
//        timer = 0f;
//        isGameActive = false;
//        gameEnded = false;
//    }

//    public void Initialize()
//    {
//        Debug.Log("Timer initialized");
//        ResetTimer();
//        isGameActive = true;
//        gameEnded = false;
//    }

//    private void Update()
//    {
//        if (!isGameActive || gameEnded) return;
//        timer += Time.deltaTime;
//        Debug.Log($"Timer: {timer}");
//        UpdateTimerDisplay();
//        if (timer >= gameDuration)
//        {
//            EndGame("Time's Up! It's a Draw!");
//            return;
//        }
//        if (!isGameActive || gameEnded) return;

//        timer += Time.deltaTime;
//        UpdateTimerDisplay();

//        if (timer >= gameDuration)
//        {
//            EndGame("Time's Up! It's a Draw!");
//            return;
//        }
//    }

//    private void UpdateTimerDisplay()
//    {
//        if (timerText == null) return;

//        int minutes = Mathf.FloorToInt(timer / 60);
//        int seconds = Mathf.FloorToInt(timer % 60);
//        timerText.text = $"{minutes:00}:{seconds:00}";
//    }

//    public void EndGame(string winner = null)
//    {
//        isGameActive = false;
//        gameEnded = true;
//        Debug.Log(winner ?? "Game Ended");
//        uiManager?.OnGameEnd();
//    }
//}

using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    [SerializeField] private float gameDuration = 90f;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI debugLogText;
    [SerializeField] private UI_Manager uiManager;

    private float timer;
    private bool isGameActive;
    private bool gameEnded;

    private void Awake()
    {
        // Cache references early
        ResetTimer();
        if (timerText == null) timerText = GetComponentInChildren<TextMeshProUGUI>();
        if (uiManager == null) uiManager = FindObjectOfType<UI_Manager>();

        // Ensure debug text is set up
        if (debugLogText == null)
        {
            GameObject debugTextObject = new GameObject("DebugLogText");
            debugTextObject.transform.SetParent(transform, false);
            debugLogText = debugTextObject.AddComponent<TextMeshProUGUI>();
            // Optional: Set up debug text positioning and styling
            RectTransform rectTransform = debugLogText.GetComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(1, 0.2f);
            rectTransform.anchoredPosition = Vector2.zero;

            debugLogText.fontSize = 14;
            debugLogText.color = Color.white;
            debugLogText.alignment = TextAlignmentOptions.BottomLeft;
        }
    }

    private void ResetTimer()
    {
        timer = 0f;
        isGameActive = false;
        gameEnded = false;
        UpdateDebugLog("Timer Reset");
    }

    public void Initialize()
    {
        UpdateDebugLog("Timer Initialized");
        ResetTimer();
        isGameActive = true;
        gameEnded = false;
    }

    private void Update()
    {
        if (!isGameActive || gameEnded) return;

        timer += Time.deltaTime;
        UpdateTimerDisplay();
        UpdateDebugLog($"Current Timer: {timer:F2}");

        if (timer >= gameDuration)
        {
            EndGame("Time's Up! It's a Draw!");
        }
    }

    private void UpdateTimerDisplay()
    {
        if (timerText == null) return;
        int minutes = Mathf.FloorToInt(timer / 60);
        int seconds = Mathf.FloorToInt(timer % 60);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    private void UpdateDebugLog(string message)
    {
        if (debugLogText == null) return;

        // Append new message to existing text, keeping a limited number of lines
        string[] existingLines = debugLogText.text.Split('\n');
        string newLogText = message + "\n";

        // Keep only last 5 lines
        if (existingLines.Length > 5)
        {
            for (int i = existingLines.Length - 5; i < existingLines.Length; i++)
            {
                newLogText += existingLines[i] + "\n";
            }
        }
        else
        {
            newLogText = debugLogText.text + message + "\n";
        }

        debugLogText.text = newLogText;
    }

    public void EndGame(string winner = null)
    {
        isGameActive = false;
        gameEnded = true;
        string endMessage = winner ?? "Game Ended";
        UpdateDebugLog(endMessage);
        uiManager?.OnGameEnd();
    }
}