using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManagerLevels : MonoBehaviour
{

    public static ManagerLevels Instance { get; protected set; }

    public GameObject islaLock;
    public GameObject islaUnlock;
    public GameObject desertlock;
    public GameObject desertUnlock;
    public GameObject castellock;
    public GameObject castelunlock;
    public GameObject cementarylock;
    public GameObject cementaryUnlock;
    public GameObject forestlock;
    public GameObject forestUnlock;
    public GameObject coming;
    
    GameState Unlock;
    

    private void Awake()
    {
        Instance = this;

    }

    private void Start()
    {
        Unlock = GameObject.FindGameObjectWithTag("GameState").GetComponent<GameState>();
    }

    private void Update()
    {
        UnlockIsland();
        UnlockDesert();
        UnlockCastel();
        UnlockCementary();
        UnlockForest();
    }

    public void UnlockIsland()
    {
        
       if(Unlock.unlockIsla == true)
        {
          //Debug.Log("Isla Desbloqueada");
            islaLock.SetActive(false);
            islaUnlock.SetActive(true);
            coming.SetActive(true);
            
        }
           
          
       
    }

    public void UnlockDesert()
    {

        if(Unlock.unlockDesert == true)
        {
          
            desertlock.SetActive(false);
            desertUnlock.SetActive(true);
        }
       
    }

    public void UnlockCastel()
    {

        if (Unlock.unlockCastel == true)
        {
            //Debug.Log("Desierto Castel");
            castellock.SetActive(false);
            castelunlock.SetActive(true);
        }

    }

    public void UnlockCementary()
    {

        if (Unlock.unlockCementary == true)
        {
            //ug.Log("Desierto Desbloqueado");
            cementarylock.SetActive(false);
            cementaryUnlock.SetActive(true);
        }

    }

    public void UnlockForest()
    {

        if (Unlock.unlockForest == true)
        {
            //ug.Log("Desierto Desbloqueado");
            forestlock.SetActive(false);
            forestUnlock.SetActive(true);
        }

    }
}
