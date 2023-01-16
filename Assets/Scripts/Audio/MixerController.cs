using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MixerController : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup audioMixer;
    /* [SerializeField] private AudioMixer effect;*/ //MaxterVolumeSFX


    public void SetVolumeSound(float slideValue)
    {
        audioMixer.audioMixer.SetFloat("MaxterBGM", Mathf.Log10(slideValue) * 20);
   
    }

    public void SetVolumeeffect(float slideValue)
    {
        audioMixer.audioMixer.SetFloat("MaxterVolumeSFX", Mathf.Log10(slideValue) * 20);
        //audio.mute = true;
       
        if (slideValue > 0.2 )
        {
           // Debug.Log("activo el salto y la caida:" + slideValue);
            Controller.Instance.FootstepPlayer.source.enabled = true;
        }
        else
        {
           //Debug.Log("activo el salto y la caida:" + slideValue);
            Controller.Instance.FootstepPlayer.source.enabled = false;
        }

    }
}
