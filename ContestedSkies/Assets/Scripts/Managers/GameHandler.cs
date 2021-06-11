using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public UIManager UIManager; // should be a singleton probably
    public Player player;

    private static float totalGameStartTime;
    private float currentGameStartTime;

    private static GameHandler instance;

    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (UIManager == null)
        {
            UIManager = GameObject.Find("UICanvas").GetComponent<UIManager>();
        }
        StartGame();
    }

    void Update()
    {
        
    }

    public static void StartGame()
    {
        instance.StartGameHelper();
    }

    private void StartGameHelper()
    {
        AudioManager.Play("Music"); // probably move this somewhere else
        currentGameStartTime = Time.time;

        player.ResetPlayer();
    }

    // for time since you hit play
    public static void SetTotalStartingTime()
    {
        totalGameStartTime = Time.unscaledTime;
    }

    public static float GetTotalStartingTime()
    {
        return Time.unscaledDeltaTime - totalGameStartTime;
    }

    // for current run
    public static float GetCurrentStartingTime()
    {
        return Time.time - instance.currentGameStartTime;
    }

    public static void GameOver()
    {
        instance.GameOverHelper();
    }

    private void GameOverHelper()
    {
        AudioManager.Play("Player Death");
        UIManager.OpenGameOver(GetCurrentStartingTime());
    }
}
