using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagerButtonsMainMenu : MonoBehaviour
{

    public GameObject options;
    public GameObject levels;
    public GameObject credits;
    public GameObject canvasMainMenu;
    public GameObject nameGame;
    public GameObject buttons;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Options()
    {
        options.SetActive(true);
        nameGame.SetActive(false);
        buttons.SetActive(false);
    }

    public void Levels()
    {
        levels.SetActive(true);
        nameGame.SetActive(false);
        buttons.SetActive(false);

    }

    public void Practice(string name)
    {
        SceneManager.LoadScene(name);
        canvasMainMenu.SetActive(false);

    }

    public void ExitOpitions()
    {
        options.SetActive(false);
        nameGame.SetActive(true);
        buttons.SetActive(true);
    }

    public void ExitLevels()
    {
        levels.SetActive(false);
        nameGame.SetActive(true);
        buttons.SetActive(true);
    }

    public void Credits()
    {
        credits.SetActive(true);
        nameGame.SetActive(false);
        buttons.SetActive(false);
    }

    public void ExirCredits()
    {
        credits.SetActive(false);
       
    }
}
