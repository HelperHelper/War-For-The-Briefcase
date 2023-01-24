using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagerScene : MonoBehaviour
{
    public static ManagerScene Instance { get; protected set; }

    [SerializeField] private GameObject loading;
    [SerializeField] private GameObject movie;
    [SerializeField] private GameObject history;
    [SerializeField] private GameObject rawHistory;
    [SerializeField] private GameObject nameGame;
    [SerializeField] private GameObject buttons;
    [SerializeField] private GameObject skipButton;
    [SerializeField] private AudioSource audioRadio;
    //[SerializeField] private GameObject menuGame;
    [SerializeField] private GameObject miraMenu;
    //[SerializeField] private GameObject clickContinue;
    [SerializeField] private GameObject mira; 
    float time = 15;
    bool historyactive;
    [HideInInspector]
    public bool island, desert, castle, cemetery, forest ;



    private void Awake()
    {
        Instance = this;
    }

    public void MainMenu(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void Practice(string name)
    {
        SceneManager.LoadScene(name);
        
    }
 
    public void Island(string name)
    {
        SceneManager.LoadScene(name);
        island = true;

    }

    public void Desert(string name)
    {
        SceneManager.LoadScene(name);
        desert = true;


    }

    public void Cementary(string name)
    {
        SceneManager.LoadScene(name);
        cemetery = true;
    }

    public void Forest(string name)
    {
        SceneManager.LoadScene(name);
        forest = true;
    }

    public void Castel(string name)
    {
        SceneManager.LoadScene(name);
        castle = true;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Update()
    {
        if(historyactive == true)
        {
            
            time -= Time.deltaTime;
           
            if (time <= 0)
            {
                skipButton.SetActive(true);
            } 
        }
    }

    public void History()
    {
        history.SetActive(true);
        rawHistory.SetActive(true);
        nameGame.SetActive(false);
        buttons.SetActive(false);
        audioRadio.enabled = false;
        historyactive = true;
    }

    public void Tutorial()
    {
        loading.SetActive(true);
        movie.SetActive(true);
        nameGame.SetActive(false);
        buttons.SetActive(false);
        audioRadio.enabled = false;

    }

    public void MenuGame()
    {
        //menuGame.SetActive(true);
        miraMenu.SetActive(true);
        nameGame.SetActive(false);
        //clickContinue.SetActive(false);
        mira.SetActive(false);
        
    }

    public void Quit()
    {
        Application.Quit();
    }

}
