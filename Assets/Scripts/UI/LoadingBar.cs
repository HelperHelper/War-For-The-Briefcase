using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingBar : MonoBehaviour
{

    [SerializeField] private Image loadBar;
    [SerializeField] private Image loadGun;
    float time = 60;
    float acounttantTime;
    // Start is called before the first frame update



    // Update is called once per frame
    void Update()
    {
        if (acounttantTime <= time)
        {
            acounttantTime = acounttantTime + Time.deltaTime;
            loadBar.fillAmount = acounttantTime / time;
            loadGun.fillAmount = acounttantTime / time;
            if (acounttantTime >= time)
            {
                Practice();
            }
        }
        
       
    }

    public void Practice()
    {
        SceneManager.LoadScene(1);
    }

    

}
