using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameSystemInfo : MonoBehaviour
{
    public static GameSystemInfo Instance { get; private set; }

    public TMP_Text TimerText;
<<<<<<< HEAD
   // public Text ScoreText;
=======
    //public Text ScoreText;
>>>>>>> master

    void Awake()
    {
        Instance = this;
    }

    public void UpdateTimer(int minutes, int seconds, int cents)
    {
        TimerText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, cents);
    }

<<<<<<< HEAD
    //public void UpdateScore(int score)
    //{
    //    ScoreText.text = score.ToString();
    //}
=======
    public void UpdateScore(int score)
    {
       // scoreText.text = score.ToString();
    }
>>>>>>> master
}

