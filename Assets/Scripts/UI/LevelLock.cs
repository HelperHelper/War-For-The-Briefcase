using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelLock : MonoBehaviour
{
  
   
    public bool isUnlock = false;
    public GameObject text;
    public float desbloqueoLevel;
    public GameObject dialogo;
    public GameObject missionMenssage;
    public TMP_Text missionMessage;



    // Start is called before the first frame update
    void Start()
    {
       
       
        
    }

    // Update is called once per frame
    void Update()
    {
     
        if (ManagerScene.Instance.island == true)
        {
            missionMessage.text = "The enemy has the location of the briefcase, use it to your advantage and bring it in.";
                
            // Debug.Log("es true");
        }

        if(ManagerScene.Instance.desert == true)
        {
            missionMessage.text = "Enemies seem to be protecting the briefcase, get it and bring it back.";
        }

        if (ManagerScene.Instance.castle == true)
        {
            missionMessage.text = "The enemy has the location of the briefcase, use it to your advantage and bring it in.";
        }
        if (ManagerScene.Instance.cemetery == true)
        {
            missionMessage.text = "Enemies seem to be protecting the briefcase, get it and bring it back.";
        }
        if (ManagerScene.Instance.forest == true)
        {
            missionMessage.text = "Enemies seem to be protecting the briefcase, get it and bring it back.";
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
