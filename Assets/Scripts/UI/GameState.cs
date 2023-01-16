using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{

    public static GameState Instance { get; protected set; }


    public static GameState gameState;
    public bool unlockIsla = false;
    public bool unlockDesert = false;
    
    
    private void Awake()
    {

        if (gameState == null)
        {
           gameState = this;
           DontDestroyOnLoad(gameObject);

        }else if(gameState!= this)
        {
            Destroy(gameObject);
        }

        Instance = this;
        
    }

   
    public void UnlockIsla()
    {

       unlockIsla = true;
               
    }

    public void UnlockDesert()
    {
       unlockDesert = true;
    }
}
