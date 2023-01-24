using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WimGameIsla : MonoBehaviour
{
    
    
   

  
    private void OnTriggerEnter(Collider other)
    {
       if (other.CompareTag("Player"))
       {
            WimPanel.Instance.WimIsla();
            GameSystem.Instance.StopTimer();
            GameSystem.Instance.FinishRun();
            //Destroy(gameObject);
        }
    }


}
