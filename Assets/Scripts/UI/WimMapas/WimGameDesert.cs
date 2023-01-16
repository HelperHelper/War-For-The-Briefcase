using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WimGameDesert : MonoBehaviour
{
    
    
   

  
    private void OnTriggerEnter(Collider other)
    {
       if (other.CompareTag("Player"))
       {
            WimPanel.Instance.WimDesert();
            GameState.Instance.UnlockDesert();
       }
    }


}
