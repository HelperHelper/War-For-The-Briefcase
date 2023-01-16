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
       
       
    }

   

   

    public void TextBriefcast()
    {
        text.SetActive(true);
        StartCoroutine(Text());
    }

    IEnumerator Text()
    {
        yield return new WaitForSeconds(5);
        text.SetActive(false);
        

    }

 
}
