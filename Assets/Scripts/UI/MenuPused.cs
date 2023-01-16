using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPused : MonoBehaviour
{

    public GameObject paused;
    public GameObject options;
   

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            paused.SetActive(true);
           
            Time.timeScale = 0;
            GameSystem.Instance.StopTimer();
            Controller.Instance.enabled = false;
           
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
        GameSystem.Instance.StartTimer();
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
    }

    public void ExitOPtions()
    {
        options.SetActive(false);
        paused.SetActive(true);
    }

    
}
