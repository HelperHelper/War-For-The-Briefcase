using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponIcon : MonoBehaviour

{


    public GameObject pistol;
    public GameObject metralleta;
    public int conteo = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") < conteo )
        {

            pistol.SetActive(false);
            metralleta.SetActive(true);
            

        }
        else if (Input.GetAxis("Mouse ScrollWheel") > conteo)
        {

            pistol.SetActive(true);
            metralleta.SetActive(false);
            

        }
    }


    
}
