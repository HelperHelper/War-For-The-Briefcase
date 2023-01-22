using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelLock : MonoBehaviour
{

   
    public bool isUnlock = false;
    public GameObject text;
    public float desbloqueoLevel;
    public GameObject dialogo;
    public GameObject missionMenssage;
    
   
    // Start is called before the first frame update
    void Start()
    {
       
       
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isUnlock == true)
        {
            
           // Debug.Log("es true");
        }

        StartCoroutine(MissionMenssage());
       
    }

   

   

    public void TextBriefcast()
    {
        text.SetActive(true);
        StartCoroutine(Text());
        dialogo.SetActive(true);
    }

    IEnumerator Text()
    {
        yield return new WaitForSeconds(5);
        text.SetActive(false);
        dialogo.SetActive(false);

    }

     IEnumerator MissionMenssage()
    {
        yield return new WaitForSeconds(8);
        missionMenssage.SetActive(false);

    }


 
}
