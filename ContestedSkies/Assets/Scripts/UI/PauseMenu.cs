using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu: MonoBehaviour
{
    public bool isGamePaused = false;

    public GameObject pausePanel;
    public Slider volumeSlider;

    public static float volume = 0.5f; // range [0..1]
    private float oldTimeScale = 1f;

    private void Awake()
    {
        volumeSlider.value = volume;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGamePaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        Time.timeScale = oldTimeScale;
        isGamePaused = false;
    }

    void PauseGame()
    {
        pausePanel.SetActive(true);
        oldTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        isGamePaused = true;
    }

    public void UpdateVolume(float value)
    {
        volume = value;
    }

    public void LoadGame()
    {
        ResumeGame();
        SceneManager.LoadScene("Game");
    }

    public void LoadMainMenu()
    {
        ResumeGame();
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quitting game!");
    }
}
