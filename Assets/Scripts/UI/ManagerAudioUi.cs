using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManagerAudioUi : MonoBehaviour
{

    public AudioSource soundTraack;
    public Slider volumenSound;
    // Start is called before the first frame update
    void Start()
    {
        soundTraack.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        soundTraack.volume = volumenSound.value;
    }

    public void PlaySound()
    {
        soundTraack.enabled = true;
    }

    public void StopSound()
    {
        soundTraack.enabled = false;
    }

    
}
