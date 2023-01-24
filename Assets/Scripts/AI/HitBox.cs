using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public Health health;

    public void OnRaycastHit(Weapon weapon, Vector3 direction)
    {
        health.TakeDamage(weapon.damage, direction);

    }

    //public void OnRaycastHitPlayer(WeaponIk weaponIk, Vector3 direction)
    //{
    //    health.TakeDamage(weaponIk.damage, direction);
    //}
}