using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public static GameOver Instance { get; protected set; }


    public MenuPused paused;
    
    // Start is called before the first frame update

    private void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
    }
    void Start()
    {
        
       
    }

    
   

    public void Retry()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //SceneManager.LoadScene(1);
        paused.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
    }

    public void GameOverPlayer()
    {

        paused.enabled = false;
        gameObject.SetActive(true);
        Controller.Instance.enabled = false;
        GameSystem.Instance.StopTimer();
        GameSystem.Instance.GameOVerRun();
        //Debug.Log("Entro al display" + gameObject.name);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
       
    }

   
    
}
