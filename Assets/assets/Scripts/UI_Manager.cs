using UnityEngine;

public class UI_Manager : MonoBehaviour
{
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private GameObject mainMenuUI;
    [SerializeField] private GameObject endGameUI;
    //[SerializeField] private GameObject gameScene; // Reference to the entire game scene or root object

    [SerializeField] private PlaneSpawn planeSpawn;
    [SerializeField] private Spawn spawn;
    [SerializeField] private GameTimer gameTimer;
    [SerializeField] private PlayerUI playerUI;

    void Start()
    {
        Application.targetFrameRate = 90;
        QualitySettings.vSyncCount = 0;
    }
    public void OnRestartPress()
    {
        ResetGame();
    }

    public void OnStartPress()
    {
        mainMenuUI.SetActive(false);
        endGameUI.SetActive(false);
        spawn?.StartSpawning();
        planeSpawn?.Initialize();
        gameTimer?.Initialize();
        playerUI?.Initialize();
        //EnableGameComponents();
        Time.timeScale = 1;
    }

    //private void EnableGameComponents()
    //{
        
        
        
        
    //}

    private void ResetGame()
    {
        // Destroy existing game objects
        DestroyGameObjects();

        // Reinitialize game components
        //EnableGameComponents();
        OnStartPress();
        Time.timeScale = 1;
    }

    public void OnGameExitPress()
    {
        ResetUIStates();
        DestroyGameObjects();
        DisableGameComponents();
        Time.timeScale = 0;
    }

    private void ResetUIStates()
    {
        endGameUI.SetActive(false);
        pauseUI.SetActive(false);
        mainMenuUI.SetActive(true);
    }

    private void DestroyGameObjects()
    {
        string[] tagsToDestroy = { "Player1", "Player2", "Asteroid" };
        foreach (string tag in tagsToDestroy)
        {
            GameObject[] objectsToDestroy = GameObject.FindGameObjectsWithTag(tag);
            for (int i = objectsToDestroy.Length - 1; i >= 0; i--)
            {
                Destroy(objectsToDestroy[i]);
            }
        }
    }

    private void DisableGameComponents()
    {
        if (planeSpawn != null) planeSpawn.enabled = false;
        if (spawn != null) spawn.enabled = false;
        if (gameTimer != null) gameTimer.enabled = false;
        if(playerUI != null) playerUI.enabled = false;
    }

    public void OnGameEnd()
    {
        endGameUI.SetActive(true);
        Time.timeScale = 0;
    }

    
    public void OnGameQuitPress() => Application.Quit();

    public void OnGamePausePress()
    {
        pauseUI.SetActive(true);
        Time.timeScale = 0;
    }

    public void OnGameResumePress()
    {
        pauseUI.SetActive(false);
        Time.timeScale = 1;
    }
}