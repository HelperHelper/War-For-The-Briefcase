using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Weapon : MonoBehaviour
{
    static RaycastHit[] hitInfoBuffer = new RaycastHit[8];
    
    public enum TriggerType
    {
        Auto,
        Manual
    }

    public enum WeaponType
    {
        Raycast,
        Projectile
    }

    public enum WeaponState
    {
        Idle,
        Firing,
        Reloading
    }

    [System.Serializable]
    public class AdvancedSettings
    {
        public float spreadAngle = 0.0f;
        public int projectilePerShot = 1;
        public float screenShakeMultiplier = 1.0f;
    }

    public TriggerType triggerType = TriggerType.Manual;
    public WeaponType weaponType = WeaponType.Raycast;
    public float fireRate = 0.5f;
    public float reloadTime = 2.0f;
    public int clipSize = 4;
    public float damage = 1.0f;
   

    [AmmoType]
    public int ammoType = -1;

    public Projectile projectilePrefab;
    public float projectileLaunchForce = 200.0f;

    public Transform EndPoint;
    public LayerMask layerMask;


    public AdvancedSettings advancedSettings;
    
    [Header("Animation Clips")]
    public AnimationClip FireAnimationClip;
    public AnimationClip ReloadAnimationClip;

    [Header("Audio Clips")]
    public AudioClip FireAudioClip;
    public AudioClip ReloadAudioClip;
    
    [Header("Visual Settings")]
    public LineRenderer PrefabRayTrail;
    public bool DisabledOnEmpty;
    
    [Header("Visual Display")]
    public AmmoDisplay AmmoDisplay;

    public bool triggerDown
    {
        get { return TriggerDown; }
        set 
        { 
            TriggerDown = value;
            if (!TriggerDown) shotDone = false;
        }
    }

    public WeaponState CurrentState => currentState;
    public int ClipContent  => clipContent ;
    public Controller Owner => owner;
    public WeaponIk OwnerTwo => ownerTwo;

    Controller owner;

    WeaponIk ownerTwo;
    
    Animator animator;
    WeaponState currentState;
    bool shotDone;
    float shotTimer = -1.0f;
    bool TriggerDown;
    int clipContent;
    bool AiWeaponIk ;

    AudioSource source;

    Vector3 convertedMuzzlePos;

    class ActiveTrail
    {
        public LineRenderer renderer;
        public Vector3 direction;
        public float remainingTime;
    }
    
    List<ActiveTrail> activeTrails = new List<ActiveTrail>();
    
    Queue<Projectile> projectilePool = new Queue<Projectile>();
    
    int fireNameHash = Animator.StringToHash("fire");
    int reloadNameHash = Animator.StringToHash("reload");     

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        source = GetComponentInChildren<AudioSource>();
        clipContent = clipSize;

        if (PrefabRayTrail != null)
        {
            const int trailPoolSize = 16;
           // Debug.Log("En el awake del arma el raytrail que tiene" + PrefabRayTrail);
            PoolSystem.Instance.InitPool(PrefabRayTrail, trailPoolSize);
        }

        if (projectilePrefab != null)
        {
            //a minimum of 4 is useful for weapon that have a clip size of 1 and where you can throw a second
            //or more before the previous one was recycled/exploded.
            int size = Mathf.Max(4, clipSize) * advancedSettings.projectilePerShot;
            for (int i = 0; i < size; ++i)
            {
                Projectile p = Instantiate(projectilePrefab);
                p.gameObject.SetActive(false);
                projectilePool.Enqueue(p);
            }
        }
    }

    public void PickedUp(Controller c)
    {
        owner = c;
        AiWeaponIk = false;
    }

    public void PickedUpTwo(WeaponIk c)
    {
        ownerTwo = c;
        AiWeaponIk = true;
    }

    public void PutAway()
    {
        if (gameObject.CompareTag("WeaponPlayer"))
        { 
            animator.WriteDefaultValues();
        }
        
        for (int i = 0; i < activeTrails.Count; ++i)
        {
            var activeTrail = activeTrails[i];
            activeTrails[i].renderer.gameObject.SetActive(false);
        }
        
        activeTrails.Clear();
    }

    public void Selected()
    {
        if (AiWeaponIk == false)
        {

            var ammoRemaining = owner.GetAmmo(ammoType);

            if (DisabledOnEmpty)
                gameObject.SetActive(ammoRemaining != 0 || clipContent != 0);

            if (FireAnimationClip != null)
                animator.SetFloat("fireSpeed", FireAnimationClip.length / fireRate);

            if (ReloadAnimationClip != null)
                animator.SetFloat("reloadSpeed", ReloadAnimationClip.length / reloadTime);

            currentState = WeaponState.Idle;

            triggerDown = false;
            shotDone = false;

            WeaponInfoUI.Instance.UpdateWeaponName(this);
            WeaponInfoUI.Instance.UpdateClipInfo(this);
            WeaponInfoUI.Instance.UpdateAmmoAmount(owner.GetAmmo(ammoType));

            if (AmmoDisplay)
                AmmoDisplay.UpdateAmount(clipContent, clipSize);

            if (clipContent == 0 && ammoRemaining != 0)
            {
                //this can only happen if the weapon ammo reserve was empty and we picked some since then. So directly
                //reload the clip when wepaon is selected
                //esto sólo puede ocurrir si la reserva de munición del arma estaba vacía y recogimos algo desde entonces. Así que directamente
                //recargar el cargador cuando se selecciona arma  
                int chargeInClip = Mathf.Min(ammoRemaining, clipSize);
                clipContent += chargeInClip;
                if (AmmoDisplay)
                    AmmoDisplay.UpdateAmount(clipContent, clipSize);
                owner.ChangeAmmo(ammoType, -chargeInClip);
                WeaponInfoUI.Instance.UpdateClipInfo(this);
            }

            animator.SetTrigger("selected");
        }
    }

    public void Fire()
    {
        if (currentState != WeaponState.Idle || shotTimer > 0 || clipContent == 0)
            return;
        
        clipContent -= 1;

      
        
        shotTimer = fireRate;

        if(AmmoDisplay)
            AmmoDisplay.UpdateAmount(clipContent, clipSize);
     

        WeaponInfoUI.Instance.UpdateClipInfo(this);

        //the state will only change next frame, so we set it right now.
        //el estado sólo cambiará en el próximo frame, así que lo establecemos ahora mismo.
        currentState = WeaponState.Firing;
        
        animator.SetTrigger("fire");

        source.pitch = Random.Range(0.7f, 1.0f);
        source.PlayOneShot(FireAudioClip);

        CameraShaker.Instance.Shake(0.2f, 0.05f * advancedSettings.screenShakeMultiplier);

        if (weaponType == WeaponType.Raycast)
        {
            for (int i = 0; i < advancedSettings.projectilePerShot; ++i)
            {
                RaycastShot();
            }
        }
        else
        {
            ProjectileShot();
        }
    }


    public void RaycastShot()
    {

        //compute the ratio of our spread angle over the fov to know in viewport space what is the possible offset from center
        //calcular el ratio de nuestro ángulo de dispersión sobre el fov para saber en espacio viewport cual es el posible desplazamiento del centro
        float spreadRatio = advancedSettings.spreadAngle / Controller.Instance.MainCamera.fieldOfView;

        Vector2 spread = spreadRatio * Random.insideUnitCircle;
        
        RaycastHit hit;
        Ray r = Controller.Instance.MainCamera.ViewportPointToRay(Vector3.one * 0.5f + (Vector3)spread);
        Vector3 hitPosition = r.origin + r.direction * 200.0f;
       // float distance = 1000.0f;


        if (Physics.Raycast(r, out hit, 1000.0f, /*~(1 << 9)*/ layerMask, QueryTriggerInteraction.Ignore))
        {
            Renderer renderer = hit.collider.GetComponentInChildren<Renderer>();
            ImpactManager.Instance.PlayImpact(hit.point, hit.normal, renderer == null ? null : renderer.sharedMaterial);

            //if too close, the trail effect would look weird if it arced to hit the wall, so only correct it if far
            //si está demasiado cerca, el efecto de estela se vería raro si se arquea para golpear la pared, así que corrígelo sólo si está lejos
            if (hit.distance > 5.0f)
                hitPosition = hit.point;
            
            // is a target
            if (hit.collider.gameObject.layer == 10)
            {
                var hitBox = hit.collider.GetComponent<HitBox>();
                if (hitBox)
                {
                    //Debug.Log("Va a dispararle al enemigo");
                    hitBox.OnRaycastHit(this, r.direction);
                }


                //Health target = hit.collider.gameObject.GetComponent<Health>();
                //target.TakeDamage(damage);
            }
        }

        //Debug.Log("que arma tiene" + ammoType);
        // si son armas de fuego que sueltan cartuchos verifica que tenga munición y suelta los cartuchos por cada disparo
        if (clipContent > 0)
        {

            // Debug.Log("Está entrando");
            ShellsController rb = GetComponent<ShellsController>();
            rb.StartShell();
        }

        if (PrefabRayTrail != null)
        {
            var pos = new Vector3[] { GetCorrectedMuzzlePlace(), hitPosition };
            var trail = PoolSystem.Instance.GetInstance<LineRenderer>(PrefabRayTrail);
            trail.gameObject.SetActive(true);
            trail.SetPositions(pos);
            activeTrails.Add(new ActiveTrail()
            {
                remainingTime = 0.3f,
                direction = (pos[1] - pos[0]).normalized,
                renderer = trail
            });
        }
    }

    void ProjectileShot()
    {
        for (int i = 0; i < advancedSettings.projectilePerShot; ++i)
        {
            float angle = Random.Range(0.0f, advancedSettings.spreadAngle * 0.5f);
            Vector2 angleDir = Random.insideUnitCircle * Mathf.Tan(angle * Mathf.Deg2Rad);

            Vector3 dir = EndPoint.transform.forward + (Vector3)angleDir;
            dir.Normalize();

            var p = projectilePool.Dequeue();
            
            p.gameObject.SetActive(true);
            p.Launch(this, dir, projectileLaunchForce);
        }

    }

    //For optimization, when a projectile is "destroyed" it is instead disabled and return to the weapon for reuse.
    public void ReturnProjecticle(Projectile p)
    {
        projectilePool.Enqueue(p);
    }

    public void Reload()
    {
        if (currentState != WeaponState.Idle || clipContent == clipSize)
            return;

        
        int remainingBullet = owner.GetAmmo(ammoType);
      
        if (remainingBullet == 0)
        {
            //No more bullet, so we disable the gun so it's displayed on empty (useful e.g. for  grenade)
            // Ya no hay bala, así que desactivamos el arma para que se muestre en vacío (útil, por ejemplo, para la granada)
            
            if (DisabledOnEmpty)
                gameObject.SetActive(false);
            return;
        }


        if (ReloadAudioClip != null)
        {
            source.pitch = Random.Range(0.7f, 1.0f);
            source.PlayOneShot(ReloadAudioClip);
        }

        int chargeInClip = Mathf.Min(remainingBullet, clipSize - clipContent);

        //the state will only change next frame, so we set it right now.
        //el estado sólo cambiará en el próximo frame, así que lo establecemos ahora mismo.
        currentState = WeaponState.Reloading;
        
        clipContent += chargeInClip;
        
        if(AmmoDisplay)
            AmmoDisplay.UpdateAmount(clipContent, clipSize);
        
        animator.SetTrigger("reload");
        
        owner.ChangeAmmo(ammoType, -chargeInClip);
        
        WeaponInfoUI.Instance.UpdateClipInfo(this);
    }

    void Update()
    {
        if (gameObject.CompareTag("WeaponPlayer"))
        {
            //Debug.Log("El jugador usa su arma y actualiza las propiedades del controlador");
        UpdateControllerState();        

        } 
        else
        if (gameObject.CompareTag("WeaponEnemy"))
        {
            UpdateControllerStateAI();
        }
        
        if (shotTimer > 0)
            shotTimer -= Time.deltaTime;

        Vector3[] pos = new Vector3[2];
        for (int i = 0; i < activeTrails.Count; ++i)
        {
            var activeTrail = activeTrails[i];
            
            activeTrail.renderer.GetPositions(pos);
            activeTrail.remainingTime -= Time.deltaTime;

            pos[0] += activeTrail.direction * 50.0f * Time.deltaTime;
            pos[1] += activeTrail.direction * 50.0f * Time.deltaTime;
            
            activeTrails[i].renderer.SetPositions(pos);
            
            if (activeTrails[i].remainingTime <= 0.0f)
            {
                activeTrails[i].renderer.gameObject.SetActive(false);
                activeTrails.RemoveAt(i);
                i--;
            }
        }
    }

    void UpdateControllerState()
    {
        if (AiWeaponIk == false)
        {
            animator.SetFloat("speed", owner.Speed);
            animator.SetBool("grounded", owner.Grounded);

            var info = animator.GetCurrentAnimatorStateInfo(0);

            WeaponState newState;
            if (info.shortNameHash == fireNameHash)
                newState = WeaponState.Firing;
            else if (info.shortNameHash == reloadNameHash)
                newState = WeaponState.Reloading;
            else
                newState = WeaponState.Idle;

            if (newState != currentState)
            {
                var oldState = currentState;
                currentState = newState;

                if (oldState == WeaponState.Firing)
                {//we just finished firing, so check if we need to auto reload
                 //acabamos de terminar de disparar, asi que comprueba si necesitamos auto recargar

                    if (clipContent == 0)
                        Reload();
                }
            }
        }

        if (triggerDown)
        {
            if (triggerType == TriggerType.Manual)
            {
                if (!shotDone)
                {
                    shotDone = true;

                    Fire();
                }
            }
            else
                Fire();
        }
    }

    void UpdateControllerStateAI()
    {
        if (triggerDown)
        {
            if (triggerType == TriggerType.Manual)
            {
                if (!shotDone)
                {
                    shotDone = true;

                    Fire();
                }
            }
            else
                Fire();
        }
    }

    /// <summary>
    /// This will compute the corrected position of the muzzle flash in world space. Since the weapon camera use a
    /// different FOV than the main camera, using the muzzle spot to spawn thing rendered by the main camera will appear
    /// disconnected from the muzzle flash. So this convert the muzzle post from
    /// world -> view weapon -> clip weapon -> inverse clip main cam -> inverse view cam -> corrected world pos
    /// Esto calculará la posición corregida del fogonazo en el espacio del mundo. Dado que la cámara del arma utiliza un
    /// diferente FOV que la cámara principal, usando el punto de la boca del cañón para generar algo renderizado por la cámara principal aparecerá
    /// desconectado del fogonazo. Asi que esto convierte el muzzle post de
    /// world -> view weapon -> clip weapon -> inverse clip main cam -> inverse view cam -> corrected world pos
    /// </summary>
    /// <returns></returns>
    public Vector3 GetCorrectedMuzzlePlace()
    {
        Vector3 position = EndPoint.position;

        position = Controller.Instance.WeaponCamera.WorldToScreenPoint(position);
        position = Controller.Instance.MainCamera.ScreenToWorldPoint(position);

        return position;
    }
}

public class AmmoTypeAttribute : PropertyAttribute
{
    
}

public abstract class AmmoDisplay : MonoBehaviour
{
    public abstract void UpdateAmount(int current, int max);
}

#if UNITY_EDITOR


[CustomPropertyDrawer(typeof(AmmoTypeAttribute))]
public class AmmoTypeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        AmmoDatabase ammoDB = GameDatabase.Instance.ammoDatabase;

        if (ammoDB.entries == null || ammoDB.entries.Length == 0)
        {
            EditorGUI.HelpBox(position, "Please define at least 1 ammo type in the Game Database", MessageType.Error);
        }
        else
        {
            int currentID = property.intValue;
            int currentIdx = -1;

            //this is pretty ineffective, maybe find a way to cache that if prove to take too much time
            string[] names = new string[ammoDB.entries.Length];
            for (int i = 0; i < ammoDB.entries.Length; ++i)
            {
                names[i] = ammoDB.entries[i].name;
                if (ammoDB.entries[i].id == currentID)
                    currentIdx = i;
            }

            EditorGUI.BeginChangeCheck();
            int idx = EditorGUI.Popup(position, "Ammo Type", currentIdx, names);
            if (EditorGUI.EndChangeCheck())
            {
                property.intValue = ammoDB.entries[idx].id;
            }
        }
    }
}

[CustomEditor(typeof(Weapon))]
public class WeaponEditor : Editor
{ 
   SerializedProperty triggerTypeProp;
   SerializedProperty weaponTypeProp;
   SerializedProperty fireRateProp;
   SerializedProperty reloadTimeProp;
   SerializedProperty clipSizeProp;
   SerializedProperty damageProp;
   SerializedProperty ammoTypeProp;
   SerializedProperty projectilePrefabProp;
   SerializedProperty projectileLaunchForceProp; 
   SerializedProperty endPointProp;
   SerializedProperty layerMaskProp;
   SerializedProperty advancedSettingsProp;
   SerializedProperty fireAnimationClipProp;
   SerializedProperty reloadAnimationClipProp;
   SerializedProperty fireAudioClipProp;
   SerializedProperty reloadAudioClipProp;
   SerializedProperty prefabRayTrailProp;
   SerializedProperty ammoDisplayProp;
   SerializedProperty disabledOnEmpty;

   void OnEnable()
   {
       triggerTypeProp = serializedObject.FindProperty("triggerType");
       weaponTypeProp = serializedObject.FindProperty("weaponType");
       fireRateProp = serializedObject.FindProperty("fireRate");
       reloadTimeProp = serializedObject.FindProperty("reloadTime");
       clipSizeProp = serializedObject.FindProperty("clipSize");
       damageProp = serializedObject.FindProperty("damage");
       ammoTypeProp = serializedObject.FindProperty("ammoType");
       projectilePrefabProp = serializedObject.FindProperty("projectilePrefab");
       projectileLaunchForceProp = serializedObject.FindProperty("projectileLaunchForce");
       endPointProp = serializedObject.FindProperty("EndPoint");
       layerMaskProp = serializedObject.FindProperty("layerMask");
       advancedSettingsProp = serializedObject.FindProperty("advancedSettings");
       fireAnimationClipProp = serializedObject.FindProperty("FireAnimationClip");
       reloadAnimationClipProp = serializedObject.FindProperty("ReloadAnimationClip");
       fireAudioClipProp = serializedObject.FindProperty("FireAudioClip");
       reloadAudioClipProp = serializedObject.FindProperty("ReloadAudioClip");
       prefabRayTrailProp = serializedObject.FindProperty("PrefabRayTrail");
       ammoDisplayProp = serializedObject.FindProperty("AmmoDisplay");
       disabledOnEmpty = serializedObject.FindProperty("DisabledOnEmpty");
   }

   public override void OnInspectorGUI()
    {
        serializedObject.Update();
        
        EditorGUILayout.PropertyField(triggerTypeProp);
        EditorGUILayout.PropertyField(weaponTypeProp);
        EditorGUILayout.PropertyField(fireRateProp);
        EditorGUILayout.PropertyField(reloadTimeProp);
        EditorGUILayout.PropertyField(clipSizeProp);
        EditorGUILayout.PropertyField(damageProp);
        EditorGUILayout.PropertyField(ammoTypeProp);

        if (weaponTypeProp.intValue == (int)Weapon.WeaponType.Projectile)
        {
            EditorGUILayout.PropertyField(projectilePrefabProp);
            EditorGUILayout.PropertyField(projectileLaunchForceProp);
        }
        
        EditorGUILayout.PropertyField(endPointProp);
        EditorGUILayout.PropertyField(layerMaskProp);
        EditorGUILayout.PropertyField(advancedSettingsProp, new GUIContent("Advance Settings"), true);
        EditorGUILayout.PropertyField(fireAnimationClipProp);
        EditorGUILayout.PropertyField(reloadAnimationClipProp);
        EditorGUILayout.PropertyField(fireAudioClipProp);
        EditorGUILayout.PropertyField(reloadAudioClipProp);

        if (weaponTypeProp.intValue == (int)Weapon.WeaponType.Raycast)
        {
            EditorGUILayout.PropertyField(prefabRayTrailProp);
        }

        EditorGUILayout.PropertyField(ammoDisplayProp);
        EditorGUILayout.PropertyField(disabledOnEmpty);

        serializedObject.ApplyModifiedProperties();
    }
}
#endif