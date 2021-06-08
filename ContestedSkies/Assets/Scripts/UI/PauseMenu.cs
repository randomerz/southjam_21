using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu: MonoBehaviour
{
    public static bool isGamePaused = false;

    public GameObject pausePanel;

    public static float volume = 1f;
    private float oldTimeScale = 1;

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

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quitting game!");
    }
}
