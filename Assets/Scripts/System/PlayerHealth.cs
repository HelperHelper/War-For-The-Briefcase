using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RendererUtils;
using UnityEngine.UI;

public class PlayerHealth : Health
{

    [Header("PlayerInfo")]
  
    public float velocityRotation = 0.05f;
    bool death;
    float time = 5f;
    
    


   
    protected override void OnStart()
    {
        UiPlayerHealthBar.Instance.UpdateMaxHealth((int)maxHealth);
        UiPlayerHealthBar.Instance.UpdateHealthInfo((int)maxHealth);

    }

    private void Update()
    {

       

        if (death == true) {

            Quaternion z = gameObject.transform.rotation;

            z.z -= velocityRotation;
            time = time + Time.deltaTime;
         

            Vector3 rotationToAdd = new Vector3(0, 0, z.z);
            transform.Rotate(rotationToAdd);
            Controller.Instance.MainCamera.fieldOfView -= 0.5f;
            

            if (time >= 13)
            {
               
                Controller.Instance.enabled = false;
                Time.timeScale = 0;
                velocityRotation = 0;
               // Debug.Log("Ha muerto y entro al if que pausa la animación: ");
                death = false;
            }

        }
    }

    protected override void OnDeath(Vector3 direction)
    {
        death = true;
        GameOver.Instance.GameOverPlayer();


        var playerbriefcase = Controller.Instance.briefcase;
        if (playerbriefcase == true)
        {

            GameSystem.Instance.StopTimer();
            GameSystem.Instance.ResetTimer();
            Instantiate(briefcase).transform.position = new Vector3(transform.position.x + 0.9f, transform.position.y + 0.5f, transform.position.z + 2f);

            playerbriefcase = false;
        }
        //AiDeathState deathState = agent.stateMachine.GetState(AiStateId.Death) as AiDeathState;
        //deathState.direction = direction;
        //agent.stateMachine.ChangeState(AiStateId.Death);
        //Debug.Log("La vida del jugador llego a 0 y a muerto");
    }

    protected override void OnDamage(Vector3 direction)
    {
       UiPlayerHealthBar.Instance.SetHealthPlayerBarPercentage(currentHealth / maxHealth);
        
        //WeaponInfoUI.Instance.UpdateAmmoAmount
        //Debug.Log("El jugador esta resiviendo daño");
    }
}
