using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiHealth : Health
{
    AiAgent agent;


    protected override void OnStart()
    {
        agent = GetComponent<AiAgent>();
    }

    protected override void OnDeath(Vector3 direction)
    {
        var enemybriefcase = Controller.Instance.enemybriefcase;
        if (enemybriefcase == true)
        {
            // Debug.Log("Entro a instanciar el maletin cuando murio");

            GameSystem.Instance.StopTimer();
            GameSystem.Instance.ResetTimer();
           // if()
            Instantiate(briefcase).transform.position = new Vector3(transform.position.x + 0.9f, transform.position.y + 0.5f, transform.position.z + 2f);

            Controller.Instance.enemybriefcase = false;
        }
        AiDeathState deathState = agent.stateMachine.GetState(AiStateId.Death) as AiDeathState;
        deathState.direction = direction;
        agent.stateMachine.ChangeState(AiStateId.Death);
    }

    protected override void OnDamage(Vector3 direction)
    {

    }
}
