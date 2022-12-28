using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Briefcase : MonoBehaviour
{
    public string briefcaseType;
    public TMP_Text briefcaseNameText;
     BriefcaseIcon briefcaseImage;
     LevelLock unlock;
    
    

    // cuando se habilita el maletin y se activa se ejecuta está función
    // mostrando la información de que es un maletin
    private void Start()
    {

        briefcaseImage = GameObject.FindGameObjectWithTag("BriefcaseIcon").GetComponent<BriefcaseIcon>();
        unlock = GameObject.FindGameObjectWithTag("LevelUnlock").GetComponent<LevelLock>();
        
    }
    void OnEnable()
    {
        briefcaseNameText.text = briefcaseType;
       
    }

    // Cuando el jugador coliciona con el maletin lo agarra y lo destruye
    void OnTriggerEnter(Collider other)
    {

        
        briefcaseImage.BriefcaseON();
        unlock.isUnlock = true;
        var briefcasechain = other.GetComponent<BriefcaseChain>();
        

        if (briefcasechain != null)
        {
            briefcasechain.GrabbedBriefcase(briefcaseType);
           // gameObject.SetActive(false);
            Destroy(gameObject);
            
        }
    }

  
}
