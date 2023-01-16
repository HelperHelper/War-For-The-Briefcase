using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiWeapons : MonoBehaviour
{
    Animator animator;
    MeshSockets sockets;
    WeaponIk weaponIk;
    Transform currentTarget;
    bool disparando;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        sockets = GetComponent<MeshSockets>();
        weaponIk = GetComponent<WeaponIk>();
    }

    //public void ActivateWeapon()
    //{
    //   // weaponIk.SetAimTransform(currentWeapon.EndPoint);
    //    //StartCoroutine(EquipWeapon());
    //}

    // Update is called once per frame
    void Update()
    {
        if (currentTarget)
        {
            Vector3 target = currentTarget.position;
        }

        if(disparando == true)
        {
            Setfiring(true);
        }
    }

    public void Setfiring(bool enabled)
    {
        if (enabled)
        {
            disparando = true;
            weaponIk.ShootControl();

        }  else {
            // parar el disparo 

            disparando = false;
        }
    }

    public void SetTarget(Transform target)
    {
        weaponIk.SetTargetTransform(target);
        currentTarget = target;
    }
}
