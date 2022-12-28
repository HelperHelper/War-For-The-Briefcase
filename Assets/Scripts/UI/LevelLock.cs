using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelLock : MonoBehaviour
{

    public GameObject levelLcok;
    public GameObject levelUnlock;
    public bool isUnlock = false;
    // Start is called before the first frame update
    void Start()
    {
      

    }

    // Update is called once per frame
    void Update()
    {
        if (isUnlock == true)
        {
            Lock();
            Debug.Log("es true");
        }
       
    }

    public void Lock()
    {
        StartCoroutine(UnlockLevel());
    }

     IEnumerator UnlockLevel()
    {
        yield return new WaitForSeconds(300);
        levelLcok.SetActive(false);
        levelUnlock.SetActive(true);
    }
}
