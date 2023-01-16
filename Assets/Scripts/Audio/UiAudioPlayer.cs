using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiAudioPlayer : MonoBehaviour
{

    public static UiAudioPlayer Instance { get; private set; }

    public AudioClip PositiveSound;
    public AudioClip NegativeSound;

    AudioSource source;

    void Awake()
    {
        source = GetComponent<AudioSource>();
        Instance = this;
    }

    public static void PlayPositive()
    {
        Instance.source.PlayOneShot(Instance.PositiveSound);
    }

    public static void PlayNegative()
    {
        Instance.source.PlayOneShot(Instance.NegativeSound);
    }
}
