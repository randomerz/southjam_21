using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WinScreenManager : MonoBehaviour
{
    public TextMeshProUGUI winTimeText;

    void Awake()
    {
        int totalSeconds = (int)GameHandler.GetTotalStartingTime();

        int mins = totalSeconds / 60;
        int secs = totalSeconds % 60;

        winTimeText.text = mins + ":" + secs.ToString("00");
    }
}
