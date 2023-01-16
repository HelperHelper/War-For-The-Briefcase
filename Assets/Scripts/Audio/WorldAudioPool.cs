using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldAudioPool : MonoBehaviour
{
    static WorldAudioPool instance;

    public AudioSource WorldSFXSourcePrefab;
    
    void Awake()
    {
        instance = this;
    }

    public static void Init()
    {
        PoolSystem.Instance.InitPool(instance.WorldSFXSourcePrefab, 32);
    }
    
    public static AudioSource GetWorldSFXSource()
    {
        return PoolSystem.Instance.GetInstance<AudioSource>(instance.WorldSFXSourcePrefab);
    }
}
