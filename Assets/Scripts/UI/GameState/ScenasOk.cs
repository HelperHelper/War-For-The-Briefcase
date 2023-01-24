using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenasOk : MonoBehaviour
{

    public int escenesCollision;

    // Start is called before the first frame update
    void Start()
    {
        escenesCollision = SceneManager.GetActiveScene().buildIndex + 1;
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (SceneManager.GetActiveScene().buildIndex == 6)
        {

        }
        else
        {
            if (other.CompareTag("Player"))
            {
                
                if (escenesCollision > PlayerPrefs.GetInt("levescen"))
                {
                    PlayerPrefs.SetInt("levescen", escenesCollision);
                }
            }
        }
    }

    public void RestarLvels()
    {
        PlayerPrefs.DeleteAll();
    }
}
