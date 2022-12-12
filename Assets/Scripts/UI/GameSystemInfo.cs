using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameSystemInfo : MonoBehaviour
{
    public static GameSystemInfo Instance { get; private set; }

    public TMP_Text TimerText;
   // public Text ScoreText;

    void Awake()
    {
        Instance = this;
    }

    public void UpdateTimer(int minutes, int seconds, int cents)
    {
        TimerText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, cents);
    }

    //public void UpdateScore(int score)
    //{
    //    ScoreText.text = score.ToString();
    //}
}

