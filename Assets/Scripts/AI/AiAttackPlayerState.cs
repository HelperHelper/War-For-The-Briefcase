using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiAttackPlayerState : AiState
{

    public AiStateId GetId()
    {
        return AiStateId.AttackPlayer;
    }

    public void Enter(AiAgent agent)
    {
        
        //agent.weapons.ActivateWeapon();
        agent.weapons.SetTarget(agent.characterTransform);
        agent.navMeshAgent.stoppingDistance = 5.0f;
        agent.weapons.Setfiring(true);

    }

    public void Update(AiAgent agent)
    {
        agent.navMeshAgent.destination = agent.characterTransform.position;
      
    }

    public void Exit(AiAgent agent)
    {
       agent.navMeshAgent.stoppingDistance = 5.0f;
    }


}
