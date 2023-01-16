using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiFindWeaponState : AiState
{
    public AiStateId GetId()
    {
        return AiStateId.FindWeapon;
    }

    public void Enter(AiAgent agent)
    {
        Weapon pickup = FindClosesWeapon(agent);
        agent.navMeshAgent.destination = pickup.transform.position;
        agent.navMeshAgent.speed = 5;

    }

    public void Update(AiAgent agent)
    {
        agent.stateMachine.ChangeState(AiStateId.AttackPlayer);
        //if (agent.weapons.HasWeapon())
        //{

        //}
    }

    public void Exit(AiAgent agent)
    {
        
    }

    private Weapon FindClosesWeapon(AiAgent agent)
    {
        Weapon[] weapons = Object.FindObjectsOfType<Weapon>();
        Weapon closesWeapon = null;
        float closestDistance = float.MaxValue;
        foreach(var weapon in weapons)
        {
            Debug.Log("Nombres de armas:" + weapon.name);
            float distanceToWeapon = Vector3.Distance(agent.transform.position, weapon.transform.position);
            if(distanceToWeapon < closestDistance)
            {
                closestDistance = distanceToWeapon;
                closesWeapon = weapon;
            }
        }
        return closesWeapon;
    }

}
