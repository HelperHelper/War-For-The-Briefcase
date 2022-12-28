using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{    
    static Collider[] sphereCastPool = new Collider[32];
    
    public bool DestroyedOnHit = true;
    public float TimeToDestroyed = 4.0f;
    public float ReachRadius = 5.0f;
    public float damage = 10.0f;
    public AudioClip DestroyedSound;
    
    //TODO : maybe pool that somewhere to not have to create one for each projectile.
    public GameObject PrefabOnDestruction;

    Weapon owner;
    Rigidbody mrigidbody;
    float timeSinceLaunch;
    
    void Awake()
    {
        PoolSystem.Instance.InitPool(PrefabOnDestruction, 4);
        mrigidbody = GetComponent<Rigidbody>();
    }

    public void Launch(Weapon launcher, Vector3 direction, float force)
    {
        owner = launcher;

        transform.position = launcher.GetCorrectedMuzzlePlace();
        transform.forward = launcher.EndPoint.forward;
        
        gameObject.SetActive(true);
        timeSinceLaunch = 0.0f;
        mrigidbody.AddForce(direction * force);
    }
    
    void OnCollisionEnter(Collision other)
    {
        if (DestroyedOnHit)
        {
            Destroy();
        }
    }

    void Destroy()
    {
        Vector3 position = transform.position;
        
        var effect = PoolSystem.Instance.GetInstance<GameObject>(PrefabOnDestruction);
        effect.transform.position = position;
        effect.SetActive(true);

        int count = Physics.OverlapSphereNonAlloc(position, ReachRadius, sphereCastPool, 1<<10);

        //for (int i = 0; i < count; ++i)
        //{
        //    Target t = sphereCastPool[i].GetComponent<Target>();
            
        //    t.Got(damage);
        //}
        
        gameObject.SetActive(false);
        mrigidbody.velocity = Vector3.zero;
        mrigidbody.angularVelocity = Vector3.zero;
        owner.ReturnProjecticle(this);

       // var source = WorldAudioPool.GetWorldSFXSource();

        //source.transform.position = position;
        //source.pitch = Random.Range(0.8f, 1.1f);
        //source.PlayOneShot(DestroyedSound);
    }

    void Update()
    {
        timeSinceLaunch += Time.deltaTime;

        if (timeSinceLaunch >= TimeToDestroyed)
        {
            Destroy();
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, ReachRadius);
    }
}
