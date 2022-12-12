using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Este Script nos permitira control sobre las escenas del juego, se podría crear un script Scenemanager y controlar las escenas desde acá
/// </summary>
public class GameSystem : MonoBehaviour
{
    public static GameSystem Instance { get; private set; }

    public GameObject[] StartPrefabs;
    public float TargetMissedPenalty = 1.0f;
    public AudioSource BGMPlayer;
    public AudioClip EndGameSound;

    public float RunTime => timer;
    public int TargetCount => targetCount;
    public int DestroyedTarget => targetDestroyed;
    public int Score => score;
    public int Minute => minutes;
    public int Second => seconds;

    float timer;
    int minutes, seconds, cents;
    bool timerRunning = false;

    int targetCount;
    int targetDestroyed;

    int score = 0;

    void Awake()
    {
        Instance = this;
        foreach (var prefab in StartPrefabs)
        {
            Instantiate(prefab);
        }

        //PoolSystem.Create();
    }

    public void ResetTimer()
    {
        timer = 0.0f;
    }

    public void StartTimer()
    {
        timerRunning = true;
    }

    public void StopTimer()
    {
        timerRunning = false;
    }

    public void FinishRun()
    {
        BGMPlayer.clip = EndGameSound;
        BGMPlayer.loop = false;
        BGMPlayer.Play();

        Controller.Instance.DisplayCursor(true);
        Controller.Instance.CanPause = false;
        //   FinalScoreUI.Instance.Display();
    }

    void Update()
    {
        if (timerRunning)
        {
            timer += Time.deltaTime;
            minutes = (int)(timer / 60f);
            seconds = (int)(timer - minutes * 60f);
            cents = (int)((timer - (int)timer) * 100f);

            GameSystemInfo.Instance.UpdateTimer(minutes, seconds, cents);
        }

        Transform playerTransform = Controller.Instance.transform;


        //UI Update
        //MinimapUI.Instance.UpdateForPlayerTransform(playerTransform);

        //if (FullscreenMap.Instance.gameObject.activeSelf)
        //    FullscreenMap.Instance.UpdateForPlayerTransform(playerTransform);
    }

    public float GetFinalTime()
    {
        int missedTarget = targetCount - targetDestroyed;

        float penalty = missedTarget * TargetMissedPenalty;

        return timer + penalty;
    }


    //public void TargetDestroyed(int score)
    //{
    //    m_TargetDestroyed += 1;
    //    m_Score += score;

    //    GameSystemInfo.Instance.UpdateScore(m_Score);
    //}
}
