using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiChaseBriefcaseState : AiState
{

    public Transform briefcaseTransform;
    public Transform doorTransform;
    //float timer = 0.0f;

    public AiStateId GetId()
    {
        return AiStateId.ChaseBriefcase;
    }

    public void Enter(AiAgent agent)
    {
        if(briefcaseTransform == null)
        {
            briefcaseTransform = GameObject.FindGameObjectWithTag("Briefcase").transform;
            agent.navMeshAgent.destination = briefcaseTransform.position;
        }

        if(doorTransform == null)
        {
            doorTransform = GameObject.FindGameObjectWithTag("Door").transform;
        }

    }

    public void Update(AiAgent agent)
    {
        //if (!agent.enabled)
        //{
        //    return;
        //}

        //timer -= Time.deltaTime;

        //if (!agent.navMeshAgent.hasPath)
        //{
        //    agent.navMeshAgent.destination = agent.characterTransform.position;
        //    agent.navMeshAgent.stoppingDistance = 5;
        //}

        //if (timer < 0.0f)
        //{
        //    var playerbriefcase = Controller.Instance.briefcase;
        //    if (playerbriefcase == true)
        //    {
        //        //Vector3 direction = (agent.characterTransform.position - agent.navMeshAgent.destination);
        //        //direction.y = 0;

        //        float sqDistance = (agent.characterTransform.position - agent.navMeshAgent.destination).sqrMagnitude;
        //        if (sqDistance > agent.config.minDistance * agent.config.minDistance) // lo que estaba dentro del if antes    direction.sqrMagnitude > agent.config.minDistance * agent.config.minDistance
        //        {
        //            //Debug.Log("El oponente tiene el maletin atrapalo");
        //            //if(agent.navMeshAgent.pathStatus != NavMeshPathStatus.PathPartial)
        //            //{

        //            agent.navMeshAgent.destination = agent.characterTransform.position;
        //            agent.navMeshAgent.stoppingDistance = 5;
        //            //}


        //        }

        //    }
            //else
            //{
                var enemybriefcase = Controller.Instance.enemybriefcase;
                if (enemybriefcase == true)
                {
                    //Debug.Log("La Ai tiene el maletin y va para la puerta");
                    
                    agent.navMeshAgent.destination = doorTransform.transform.position;
                    agent.navMeshAgent.stoppingDistance = 5f;
                    agent.stateMachine.ChangeState(AiStateId.Idle);
        }
                else
                {
                    var playerbriefcase = Controller.Instance.briefcase;
                    if (playerbriefcase == true)
                    {
                      agent.stateMachine.ChangeState(AiStateId.ChasePlayer);
                    }
                }
        //}
        //timer = agent.config.maxTime;
        //}
    }

    public void Exit(AiAgent agent)
    {
    }


}
