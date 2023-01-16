using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Briefcase : MonoBehaviour
{
    public string briefcaseType;
    public TMP_Text briefcaseNameText;
    public GameObject briefcase;
    BriefcaseIcon briefcaseImage;
    LevelLock unlock;

    private void Start()
    {
        briefcaseImage = GameObject.FindGameObjectWithTag("BriefcaseIcon").GetComponent<BriefcaseIcon>();
        unlock = GameObject.FindGameObjectWithTag("LevelUnlock").GetComponent<LevelLock>();
    }

    // cuando se habilita el maletin y se activa se ejecuta está función
    // mostrando la información de que es un maletin
    void OnEnable()
    {
        briefcaseNameText.text = briefcaseType;
    }

    // Cuando el jugador coliciona con el maletin lo agarra y lo destruye
    void OnTriggerEnter(Collider other)
    {

        unlock.TextBriefcast();
        briefcaseImage.BriefcaseON();


        unlock.isUnlock = true;

        var briefcasechain = other.GetComponent<BriefcaseChain>();

        if (briefcasechain != null)
        {
            briefcasechain.GrabbedBriefcase(briefcaseType);
            // gameObject.SetActive(false);
            if (other.CompareTag("Player"))
            {
               //Debug.Log("El jugador recogio el maletin");
                Controller.Instance.briefcase = true;
                Destroy(gameObject);

            }
            else
            if (other.CompareTag("Enemy"))
            {
               // Debug.Log("El Enemigo recogio el maletin");
                Controller.Instance.enemybriefcase = true;
                Destroy(gameObject);
            }
        } 
    }
}
