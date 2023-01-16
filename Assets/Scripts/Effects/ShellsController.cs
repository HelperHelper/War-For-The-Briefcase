using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellsController : MonoBehaviour
{ 

    public ParticleSystem shell;


    public void StartShell()
    {
       // ParticleSystem shell = GetComponent<ParticleSystem>();
        // Debug.Log("Está entrando");
        shell.Play(); 

    }

    public void StopShell()
    {
       // ParticleSystem shell = GetComponent<ParticleSystem>();
        //Debug.Log("Paro la animación de particula");
        shell.Stop();

    }

}
