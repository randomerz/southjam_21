using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public UIManager UIManager; // should be a singleton probably
    public Player player;

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

        player.ResetPlayer();
    }

    public static void GameOver()
    {
        instance.GameOverHelper();
    }

    private void GameOverHelper()
    {
        AudioManager.Play("Player Death");
        UIManager.OpenGameOver(69);
    }
}
