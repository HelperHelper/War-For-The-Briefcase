using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WimPanel : MonoBehaviour
{

    public static WimPanel Instance { get; protected set; }

    public GameObject isla;
    public GameObject desert;
    public GameObject castel;
    public GameObject cementary;
    public GameObject forest;
    public GameObject unlockIsla;
   
    //public GameObject regaloDesbloqueado;
    public MenuPused paused;
    
    private void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
    }
    // Start is called before the first frame update
    //void Start()
    //{
        
    //}

    public void WimIsla()
    {

        paused.enabled = false;
       // regaloDesbloqueado.SetActive(true);
        isla.SetActive(true);
        gameObject.SetActive(true);
       //ebug.Log("Ganaste" + gameObject.name);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void WimDesert()
    {

        paused.enabled = false;
       // regaloDesbloqueado.SetActive(true);
        desert.SetActive(true);
        
        gameObject.SetActive(true);
      //Debug.Log("Ganaste" + gameObject.name);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void WimCastel()
    {

        paused.enabled = false;
        // regaloDesbloqueado.SetActive(true);
        castel.SetActive(true);
        gameObject.SetActive(true);
        
        //Debug.Log("Ganaste" + gameObject.name);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void WimCementary()
    {

        paused.enabled = false;
        // regaloDesbloqueado.SetActive(true);
        cementary.SetActive(true);
        gameObject.SetActive(true);
        //Debug.Log("Ganaste" + gameObject.name);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void WimForest()
    {

        paused.enabled = false;
        // regaloDesbloqueado.SetActive(true);
        forest.SetActive(true);
        gameObject.SetActive(true);
        //Debug.Log("Ganaste" + gameObject.name);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    
    
    public void MainMenuPractice()
    {

        SceneManager.LoadScene(0);
        paused.enabled = true;
       

    }

 

}
