using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading_screen : MonoBehaviour
{
    public GameObject loadingScreen; // Reference to the loading screen UI
    public Slider progressBar;       // Slider for progress bar
    public Text progressText;        // Text to display loading percentage

    // Call this method to load a new scene
    public void LoadScene(string sceneName)
    {
        loadingScreen.SetActive(true); // Enable loading screen UI
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    // Coroutine to load scene asynchronously
    private IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f); // Normalize progress to 0-1
            progressBar.value = progress; // Update progress bar
            progressText.text = Mathf.RoundToInt(progress * 100) + "%"; // Update percentage text

            // Check if loading is almost complete
            if (operation.progress >= 0.9f)
            {
                progressText.text = "Tap to Continue";
                if (Input.anyKeyDown)
                {
                    operation.allowSceneActivation = true; // Activate the scene when player taps
                }
            }
            yield return null;
        }
    }
}
