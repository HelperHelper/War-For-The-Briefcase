using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBox : MonoBehaviour
{
    public float maxHealtBox;
 
    void Reset()
    {
        gameObject.layer = LayerMask.NameToLayer("PlayerCollisionOnly");
        GetComponent<Collider>().isTrigger = true;

    }
    void OnTriggerEnter(Collider other)
    {
       /// Debug.Log("Entra al collider de la vida");
          Health h = other.GetComponent<Health>();

            if(h.setAlphaBlood.a <= 0)
            {
                h.setAlphaBlood.a = 0f;
            }

          if (h.currentHealth + maxHealtBox > h.maxHealth)
          {
            h.currentHealth = h.maxHealth;
            h.setAlphaBlood.a -= 1f;
            UiPlayerHealthBar.Instance.blood.color = h.setAlphaBlood;
            //Debug.Log("Recogimos vida sumo en if" + h.setAlphaBlood);

            UiPlayerHealthBar.Instance.SetHealthPlayerBarPercentage(h.currentHealth / h.maxHealth);
           }
          else
          {
            h.currentHealth += maxHealtBox;
            h.setAlphaBlood.a -= 0.3f;
            UiPlayerHealthBar.Instance.blood.color = h.setAlphaBlood;
            UiPlayerHealthBar.Instance.SetHealthPlayerBarPercentage(h.currentHealth / h.maxHealth);
          }
            UiPlayerHealthBar.Instance.UpdateHealthInfo((int)h.currentHealth);
  
  
  
    }
    /* void OnTriggerEnter(Collider other)
    {
        Health h = other.GetComponent<Health>();
        Controller c = other.GetComponent<Controller>();
        UiPlayerHealthBar.Instance.UpdateHealthInfo((int)h.maxHealth + 300);
    } */

 


}
