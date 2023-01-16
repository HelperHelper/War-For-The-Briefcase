using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagerScene : MonoBehaviour
{


    [SerializeField] private GameObject loading;
    [SerializeField] private GameObject movie;
    [SerializeField] private GameObject nameGame;
    [SerializeField] private GameObject buttons;
    [SerializeField] private AudioSource audioRadio;

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
    }

    public void Desert(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void Cementary(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void Forest(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void Castel(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Tutorial()
    {
        loading.SetActive(true);
        movie.SetActive(true);
        nameGame.SetActive(false);
        buttons.SetActive(false);
        audioRadio.enabled = false;

    }
}
