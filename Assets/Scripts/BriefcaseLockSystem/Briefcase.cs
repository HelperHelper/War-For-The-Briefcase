using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Briefcase : MonoBehaviour
{
    public string briefcaseType;
    public TMP_Text briefcaseNameText;

    // cuando se habilita el maletin y se activa se ejecuta está función
    // mostrando la información de que es un maletin
    void OnEnable()
    {
        briefcaseNameText.text = briefcaseType;
    }

    // Cuando el jugador coliciona con el maletin lo agarra y lo destruye
    void OnTriggerEnter(Collider other)
    {
        var briefcasechain = other.GetComponent<BriefcaseChain>();

        if (briefcasechain != null)
        {
            briefcasechain.GrabbedBriefcase(briefcaseType);
           // gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
