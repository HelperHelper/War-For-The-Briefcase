using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BriefcaseChain : MonoBehaviour
{
    List<string> m_BriefcaseTypeOwned = new List<string>();

    // metodo para saber si cogio el maletin
    public void GrabbedBriefcase(string briefcaseType)
    {
        m_BriefcaseTypeOwned.Add(briefcaseType);
    }

    // metodo para verificar que sigue teniendo el maletin o dispone del maletin
    public bool HaveBriefcase(string briefcaseType)
    {
        return m_BriefcaseTypeOwned.Contains(briefcaseType);
    }

    // metodo que permite usar el maletin y luego de usarlo removerlo
    public void UseKey(string briefcaseType)
    {
        m_BriefcaseTypeOwned.Remove(briefcaseType);
    }
}
