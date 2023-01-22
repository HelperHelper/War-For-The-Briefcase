using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RendererUtils;
using UnityEngine.UI;

public class PlayerHealth : Health
{

    public static PlayerHealth Instance { get; private set; }

    [Header("PlayerInfo")]
  
    public float velocityRotation = 0.05f;
    bool death;
    [HideInInspector] public bool playerdeath;
    float time = 5f;

    public void Awake()
    {
        Instance = this;
    }

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
            
            HitPlayer.source.Stop();
            HitPlayer.source.pitch = 0;

            if (time >= 13)
            {
               
                Controller.Instance.enabled = false;
              
                velocityRotation = 0;
               // Debug.Log("Ha muerto y entro al if que pausa la animación: ");
                death = false;
                playerdeath = true;
            }

        }
    }

    protected override void OnDeath(Vector3 direction)
    {
        death = true;
        GameOver.Instance.GameOverPlayer();
        Time.timeScale = 0;

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
