using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiChasePlayerState : AiState
{
    float timer = 0.0f;

    public AiStateId GetId()
    {
        return AiStateId.ChasePlayer;
    }

    public void Enter(AiAgent agent)
    {
       
    }
    public void Update(AiAgent agent)
    {
        if (!agent.enabled)
        {
            return;
        }

        timer -= Time.deltaTime;

        if (!agent.navMeshAgent.hasPath)
        {
            agent.navMeshAgent.destination = agent.characterTransform.position;
            agent.navMeshAgent.stoppingDistance = 5;
        }

        if (timer < 0.0f)
        {
          
                //Vector3 direction = (agent.characterTransform.position - agent.navMeshAgent.destination);
                //direction.y = 0;

                float sqDistance = (agent.characterTransform.position - agent.navMeshAgent.destination).sqrMagnitude;
                if (sqDistance > agent.config.minDistance * agent.config.minDistance) // lo que estaba dentro del if antes    direction.sqrMagnitude > agent.config.minDistance * agent.config.minDistance
                {
                    //Debug.Log("El oponente tiene el maletin atrapalo");
                    //if(agent.navMeshAgent.pathStatus != NavMeshPathStatus.PathPartial)
                    //{

                    agent.navMeshAgent.destination = agent.characterTransform.position;
                    agent.navMeshAgent.stoppingDistance = 5;

                    //agent.stateMachine.ChangeState(AiStateId.AttackPlayer);

            
                //}

                }


            if (PlayerHealth.Instance.playerdeath == true)
            {
                agent.weapons.Setfiring(false);
            }
            else
            {
                agent.weapons.Setfiring(true);
            }

            timer = agent.config.maxTime;
        }

     }

    public void Exit(AiAgent agent)
    {
        
    }

  

}
