using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimationEventHandler : MonoBehaviour
{
    Weapon owner;

    void Awake()
    {
        owner = GetComponentInParent<Weapon>();
    }

    public void PlayFootstep()
    {
        owner.Owner.PlayFootstep();
    }
}
