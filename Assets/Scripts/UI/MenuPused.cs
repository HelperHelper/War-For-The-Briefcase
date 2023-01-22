using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPused : MonoBehaviour
{

    public GameObject paused;
    public GameObject options;
    public GameObject credits;
    bool activeOptions;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            paused.SetActive(true);
           
            Time.timeScale = 0;
            GameSystem.Instance.StopTimer();
            Controller.Instance.enabled = false;
           if(activeOptions == true)
            {
                options.SetActive(false);
            }
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Resume()
    {

        Time.timeScale = 1;
        Controller.Instance.enabled = true;
        // GameSystem.Instance.
        if (Controller.Instance.briefcase == true || Controller.Instance.enemybriefcase == true)
        {
            GameSystem.Instance.StartTimer();
        }
        UiAudioPlayer.PlayPositive();
        paused.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Options()
    {
        options.SetActive(true);
        UiAudioPlayer.PlayPositive();
        paused.SetActive(false);
        activeOptions = true;
    }

    public void ExitOPtions()
    {
        options.SetActive(false);
        paused.SetActive(true);
    }

    public void Credits()
    {
        credits.SetActive(true);
        paused.SetActive(false);
        options.SetActive(false);
    }

    public void ExitCredits()
    {
        credits.SetActive(false);
        paused.SetActive(false);
        options.SetActive(true);
    }

}
