using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using  UnityEngine.UI;

public class StatusPlayer : MonoBehaviour
{
    public static StatusPlayer Instance{get; protected set;}
    // Start is called before the first frame update
     Image healthyPlayer;
     Image injuredPlayer1;
     Image injuredPlayer2;
     Image injuredPlayer3;
    
    // public Health h;

    
    void Awake() {
        Instance = this;
    }
    void Start()
    {
        // h = FindObjectOfType<Health>();
        // h = GetComponent<Health>();
      healthyPlayer  = GameObject.FindGameObjectWithTag("Status1").GetComponent<Image>();
      injuredPlayer1 = GameObject.FindGameObjectWithTag("Status2").GetComponent<Image>();
      injuredPlayer2 = GameObject.FindGameObjectWithTag("Status3").GetComponent<Image>();
      injuredPlayer3 = GameObject.FindGameObjectWithTag("Status4").GetComponent<Image>();
     
    }

    public void StatusPlayerBar(float currentHealth, float maxHealth){
               
             if (currentHealth >= maxHealth * 0.75f)
                {
                    healthyPlayer.enabled = true;
                    injuredPlayer1.enabled = false;
                    injuredPlayer2.enabled = false;
                    injuredPlayer3.enabled = false;
                    
                }
                else if (currentHealth < maxHealth * 0.75f && currentHealth >= maxHealth * 0.5f)
                {
                   
                    healthyPlayer.enabled = false;
                    injuredPlayer1.enabled = true;
                    injuredPlayer2.enabled = false;
                    injuredPlayer3.enabled = false;
                }
                else if (currentHealth < maxHealth * 0.5f && currentHealth >= maxHealth * 0.25f)
                {
                
                    healthyPlayer.enabled = false;
                    injuredPlayer1.enabled = false;
                    injuredPlayer2.enabled = true;
                    injuredPlayer3.enabled = false;
                }
                else if (currentHealth < maxHealth * 0.25f)
                {
                    
                    healthyPlayer.enabled = false;
                    injuredPlayer1.enabled = false;
                    injuredPlayer2.enabled = false;
                    injuredPlayer3.enabled = true;
                }
                 
    }
public void StatusPlayerHealth(float currentHealth, float maxHealth){


                 if (currentHealth >= maxHealth * 0.75f)
                {
                    healthyPlayer.enabled = true;
                    injuredPlayer1.enabled = false;
                    injuredPlayer2.enabled = false;
                    injuredPlayer3.enabled = false;
                     
                }
                else if (currentHealth >= maxHealth * 0.5f)
                {
                    
                    healthyPlayer.enabled = false;
                    injuredPlayer1.enabled = true;
                    injuredPlayer2.enabled = false;
                    injuredPlayer3.enabled = false;
                }
                else if (currentHealth >= maxHealth * 0.25f)
                {
                    
                    healthyPlayer.enabled = false;
                    injuredPlayer1.enabled = false;
                    injuredPlayer2.enabled = true;
                    injuredPlayer3.enabled = false;
                }
            

}

}
