using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HumanBone
{
    public HumanBodyBones bone;
    public float weight = 1.0f;
}


public class WeaponIk : MonoBehaviour
{
    public static WeaponIk Instance { get; private set; }

    public Transform targetTransform;
    public Transform aimTransform;
    //public Collider playerCollider;
    //public Transform bone;
    public Vector3 targetOffset;
    public LineRenderer PrefabRayTrail;

    public int iterations = 10;
    [Range(0,1)]
    public float weight = 1.0f;
    public float angleLimit = 90.0f;
    public float distanceLimit = 1.5f;
    public float shootDistance = 50f;
    public float shootInternal = 2f;
    public float shootRealTime = 3f;
    public float bulletSpeed = 1000.0f;
    float shootTime;
    float interval;
    float distanceToTarget;
    public float damage = 1.0f;

    [System.Serializable]
    public class AdvancedSettings
    {
        public float spreadAngle = 0.0f;
        public int projectilePerShot = 1;
        public float screenShakeMultiplier = 1.0f;
    }

    public HumanBone[] humanBones;
    public AdvancedSettings advancedSettings;
    Transform[] boneTransforms;


    class ActiveTrail
    {
        public LineRenderer renderer;
        public Vector3 direction;
        public float remainingTime;
    }

    List<ActiveTrail> activeTrails = new List<ActiveTrail>();

    public void Awake()
    {
        Instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        Animator animator = GetComponent<Animator>();
        boneTransforms = new Transform[humanBones.Length];
        for(int i = 0; i < boneTransforms.Length; i++)
        {
            boneTransforms[i] = animator.GetBoneTransform(humanBones[i].bone);
        }

        if (PrefabRayTrail != null)
        {
            const int trailPoolSize = 16;
            //Debug.Log("Cuando instancio y llamo al pool me tira error de instancia");
            // Debug.Log("En el awake del arma el raytrail que tiene" + PrefabRayTrail);
            PoolSystem.Instance.InitPool(PrefabRayTrail, trailPoolSize);
        }

        shootTime = shootInternal;
        interval = shootInternal;
    }


    Vector3 GetTargetPosition()
    {
        Vector3 targetDirection = (targetTransform.position + targetOffset) - aimTransform.position;
        Vector3 aimDirection = aimTransform.forward;
        float blendOut = 0.0f;

        float targetAngle = Vector3.Angle(targetDirection, aimDirection);
        if(targetAngle > angleLimit)
        {
            blendOut += (targetAngle - angleLimit) / 50.0f;
        }

        float targetDistance = targetDirection.magnitude;
        if(targetDistance < distanceLimit)
        {
            blendOut += distanceLimit - targetDistance;
        }

        Vector3 direction = Vector3.Slerp(targetDirection, aimDirection, blendOut);
        return aimTransform.position + direction;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(aimTransform == null)
        {
            return;
        }

        if(targetTransform == null)
        {
            return;
        }

        Vector3 targetPosition = GetTargetPosition(); // targetTransform.position; // se sustituye por GetTargetPosition()
        for (int i = 0; i < iterations; i++)
        {
            for (int b = 0; b < boneTransforms.Length; b++)
            {
                Transform bone = boneTransforms[b];
                float boneWeight = humanBones[b].weight * weight;
                //Debug.Log("Que tiene bone" + bone);
                //Debug.Log("Que tiene boneWeight" + boneWeight);

            AimAtTarget(bone, targetPosition, boneWeight); // se sustituye weight por boneWeight
            }

        }


        distanceToTarget = Vector3.Distance(transform.position, targetTransform.position);
       // ShootControl();


   



    }


    public void ShootControl()
    {
        shootRealTime -= Time.deltaTime;

        // Debug.Log("Tiempo de shootTime" + shootTime);
        if (shootRealTime < 0)
        {
            if (distanceToTarget < shootDistance)
            {


                float spreadRatio = advancedSettings.spreadAngle;

               // Vector2 spread = spreadRatio * UnityEngine.Random.insideUnitCircle;
                //  Ray r = Controller.Instance.MainCamera.ViewportPointToRay(Vector3.one * 0.5f + (Vector3)spread);
                RaycastHit hit;
                  //  Ray r = new Ray(aimTransform.position, transform.forward /*+ (Vector3)spread*/);
                    Ray r = new Ray(aimTransform.position, aimTransform.forward /** 0.5f*/ /*+ (Vector3)spread*/);
                    Vector3 hitPosition = r.origin + r.direction * 200.0f;
           
                if (Physics.Raycast(r, out hit, 1000.0f, ~(1 << 9), QueryTriggerInteraction.Ignore))
                    {
                    //Renderer renderer = hit.collider.GetComponentInChildren<Renderer>();
                   
                    if (hit.distance > 5.0f)
                            hitPosition = hit.point;
                    shootRealTime = shootTime;

                   
                        // is a target
                        if (hit.collider.gameObject.layer == 12)
                        {
                           //Debug.Log("Sigue entrando a dispararle al jugador");
                            //ImpactManager.Instance.PlayImpact(hit.point, hit.normal, renderer == null ? null : renderer.sharedMaterial);
                           // Debug.Log("Le esta disparando al jugador " + r.direction);
                            Health target = hit.collider.gameObject.GetComponent<Health>();
                            target.TakeDamage(damage, r.direction);
                            //var hitBox = hit.collider.GetComponent<HitBox>();
                            //if (hitBox)
                            //{
                            //    Debug.Log("Va a dispararle al jugador le envia: " + this + r.direction);
                            //    hitBox.OnRaycastHitPlayer(this, r.direction);
                            //}

                            //Health target = hit.collider.gameObject.GetComponent<Health>();
                            //target.TakeDamage(damage);

                        }
                }
          
                    if (PrefabRayTrail != null)
                    {
                        // Debug.Log("Entra a disparar");
                        var pos = new Vector3[] { GetCorrectedMuzzlePlace(), hitPosition };
                        var trail = PoolSystem.Instance.GetInstance<LineRenderer>(PrefabRayTrail);
                        trail.gameObject.SetActive(true);
                        trail.SetPositions(pos);
                        activeTrails.Add(new ActiveTrail()
                        {
                            remainingTime = 0.02f,
                            direction = (pos[1] - pos[0]).normalized,
                            // direction = (aimTransform.position - targetTransform.position).normalized * bulletSpeed,
                            renderer = trail
                        });
                    }


                if (interval < 0)
                    interval -= Time.deltaTime;

                Vector3[] posone = new Vector3[2];
                for (int i = 0; i < activeTrails.Count; ++i)
                {
                    var activeTrail = activeTrails[i];

                    activeTrail.renderer.GetPositions(posone);
                    activeTrail.remainingTime -= Time.deltaTime;

                    posone[0] += activeTrail.direction * 50.0f * Time.deltaTime;
                    posone[1] += activeTrail.direction * 50.0f * Time.deltaTime;

                    activeTrails[i].renderer.SetPositions(posone);

                    if (activeTrails[i].remainingTime <= 0f)
                    {
                        activeTrails[i].renderer.gameObject.SetActive(false);
                        activeTrails.RemoveAt(i);
                        i--;
                    }
                }

            }
        }
    }


   //void Update()
   // {
     
           
       
   // }




    private void AimAtTarget(Transform bone, Vector3 targetPosition, float weight)
    {
        Vector3 aimDirection = aimTransform.forward;
        Vector3 targetDirection = targetPosition - aimTransform.position;
        Quaternion aimTowards = Quaternion.FromToRotation(aimDirection, targetDirection);
        Quaternion blendedRotation = Quaternion.Slerp(Quaternion.identity, aimTowards, weight);
        bone.rotation = blendedRotation * bone.rotation;
    }

    public Vector3 GetCorrectedMuzzlePlace()
    {
        Vector3 position = aimTransform.position /*+ transform.forward*50*/;
        position = aimTransform.position + aimTransform.forward;

        //position = Controller.Instance.WeaponCamera.WorldToScreenPoint(position);
        //position = Controller.Instance.MainCamera.ScreenToWorldPoint(position);

        return position;
    }

    public void SetTargetTransform(Transform target)
    {
        targetTransform = target;
    }

    public void SetAimTransform(Transform aim)
    {
        aimTransform = aim;
    }
}
