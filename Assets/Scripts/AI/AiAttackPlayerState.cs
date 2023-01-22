using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiAttackPlayerState : AiState
{
    public Transform doorTransform;

    public AiStateId GetId()
    {
        return AiStateId.AttackPlayer;
    }

    public void Enter(AiAgent agent)
    {
        
        //agent.weapons.ActivateWeapon();
        agent.weapons.SetTarget(agent.characterTransform);
        agent.navMeshAgent.stoppingDistance = 5.0f;

        //Debug.Log("Que contiene playerHealth" + PlayerHealth.Instance.playerdeath);

        if (PlayerHealth.Instance.playerdeath == true)
        {
            agent.weapons.Setfiring(false);
        }
        else
        {
        agent.weapons.Setfiring(true);
        }

        if (doorTransform == null)
        {
            doorTransform = GameObject.FindGameObjectWithTag("Door").transform;
        }

    }

    public void Update(AiAgent agent)
    {
        //Debug.Log("Entra al debug de seguir y atacar al jugador");
        if (PlayerHealth.Instance.playerdeath == true)
        {
            agent.navMeshAgent.destination = doorTransform.transform.position;
            agent.navMeshAgent.stoppingDistance = 5f;
            agent.stateMachine.ChangeState(AiStateId.Idle);
        }
        else
        {
           // Debug.Log("va a atacar al jugador");

          agent.navMeshAgent.destination = agent.characterTransform.position;
        }
        
      
    }

    public void Exit(AiAgent agent)
    {
       agent.navMeshAgent.stoppingDistance = 5.0f;
    }


}
