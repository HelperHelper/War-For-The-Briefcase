using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerNav : MonoBehaviour
{


    public NavMeshAgent navMeshAgent;
    public GameObject goalDestination;
    public GameObject character;
    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent.destination = goalDestination.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        var playerbriefcase = Controller.Instance.briefcase;

        //Debug.Log("Posicion del jugador: " + character.transform.position);

        if (playerbriefcase == true)
        {
          //Debug.Log("El oponente tiene el maletin atrapalo");
            navMeshAgent.destination = character.transform.position;
        }
    }
}
