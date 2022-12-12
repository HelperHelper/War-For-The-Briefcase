using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BriefcaseChain : MonoBehaviour
{
    List<string> briefcaseTypeOwned = new List<string>();

    // metodo para saber si cogio el maletin
    public void GrabbedBriefcase(string briefcaseType)
    {
        briefcaseTypeOwned.Add(briefcaseType);
        //Debug.Log("Recogio el maletin y ahora lo tiene el jugador");
        Controller.Instance.briefcase = true;
        GameSystem.Instance.StartTimer();
    }

    // metodo para verificar que sigue teniendo el maletin o dispone del maletin cuando intenta acceder a la puerta
    public bool HaveBriefcase(string briefcaseType)
    {
       // Debug.Log("Tiene el maletin y lo sigue teniendo");
        return briefcaseTypeOwned.Contains(briefcaseType);
       
    }

    // metodo que permite usar el maletin y luego de usarlo removerlo
    public void UseKey(string briefcaseType)
    {
        briefcaseTypeOwned.Remove(briefcaseType);
    }
}
