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