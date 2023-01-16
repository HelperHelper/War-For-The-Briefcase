using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;


public class Health : MonoBehaviour
{


    public float maxHealth;
    public int pointValue;
    public ParticleSystem DestroyedEffect;
    SkinnedMeshRenderer skinnedMeshRenderer;
    UIHealthBar healthBar;
    public GameObject briefcase;
    public float dieMaxTime = 25f;

    [HideInInspector]
    public float currentHealth;
    [HideInInspector]
    public Color setAlphaBlood;
    // AiAgent agent;

    //public float blinkIntesity;
    //public float blinkDuration;
    //float blinkTimer;

    [Header("Audio")]
    public RandomPlayer HitPlayer;
    public AudioSource IdleSource;

    public bool Destroyed => destroyed;

    bool destroyed = false;
   

    // Start is called before the first frame update
    void Start()
    {
        //skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        healthBar = GetComponentInChildren<UIHealthBar>();
        //if (DestroyedEffect)
         PoolSystem.Instance.InitPool(DestroyedEffect, 16);

        //agent = GetComponent<AiAgent>();
        currentHealth = maxHealth;
     
        var rigidBodies = GetComponentsInChildren<Rigidbody>();
        foreach(var rigidBody in rigidBodies)
        {
          HitBox hitBox =  rigidBody.gameObject.AddComponent<HitBox>();
          hitBox.health = this;  
            //if(hitBox.gameObject != gameObject)
            //{
            //    hitBox.gameObject.layer = LayerMask.NameToLayer("Player");
            //}
        }

        OnStart();

        if (IdleSource != null)
            IdleSource.time = Random.Range(0.0f, IdleSource.clip.length);
    }

    //metodo nuevo para quitar vida mediante projectil ya que el daño por raycast es diferente y pide un parametro extra que no necesita el projectil
    public void ProjectileTakeDamage(float damage)
    {
        currentHealth -= damage;
        healthBar.SetHealthBarPercentage(currentHealth / maxHealth);
        if (currentHealth <= 0.0f)
        {
            var enemybriefcase = Controller.Instance.enemybriefcase;
            if (enemybriefcase == true)
            {


                GameSystem.Instance.StopTimer();
                GameSystem.Instance.ResetTimer();
                Instantiate(briefcase).transform.position = new Vector3(transform.position.x, transform.position.y + 0.3f, transform.position.z);

                enemybriefcase = false;
            }
            Vector3 direction = transform.position;
            Die(direction);
        }

        //blinkTimer = blinkDuration;

       

        if (HitPlayer != null)
            HitPlayer.PlayRandom();





        if (currentHealth > 0)
            return;

        Vector3 position = transform.position;

        //the audiosource of the target will get destroyed, so we need to grab a world one and play the clip through it
        // La fuente de audio del objetivo será destruida, por lo que necesitamos tomar una del mundo y reproducir el clip a través de ella.
        if (HitPlayer != null)
        {
            var source = WorldAudioPool.GetWorldSFXSource();
            source.transform.position = position;
            source.pitch = HitPlayer.source.pitch;
            source.PlayOneShot(HitPlayer.GetRandomClip());
        }

        if (DestroyedEffect != null)
        {
            var effect = PoolSystem.Instance.GetInstance<ParticleSystem>(DestroyedEffect);
            effect.time = 0.0f;
            effect.Play();
            effect.transform.position = position;
        }

        destroyed = true;

        gameObject.SetActive(false);
        healthBar.gameObject.SetActive(false);

        GameSystem.Instance.TargetDestroyed(pointValue);




    }

    public void TakeDamage(float damage, Vector3 direction)
    {
        currentHealth -= damage;
        
        if (currentHealth <= 0.0f && destroyed == false)
        {
            Die(direction);
            destroyed = true;
        }

        if (gameObject.CompareTag("Enemy"))
        {
            healthBar.SetHealthBarPercentage(currentHealth / maxHealth);
        }else
         if (gameObject.CompareTag("Player"))
        {
            if (currentHealth >= 0)
            {
                UiPlayerHealthBar.Instance.UpdateHealthInfo(((int)(currentHealth)));
                setAlphaBlood = UiPlayerHealthBar.Instance.blood.color;
                setAlphaBlood.a += 0.02f;
                if (setAlphaBlood.a >= 1f)
                {
                    setAlphaBlood.a = 1f;
                }
                UiPlayerHealthBar.Instance.blood.color = setAlphaBlood;
                //UiPlayerHealthBar.Instance.setAlphaBlood += 1f;
            }
        }


        OnDamage(direction);

        Vector3 position = transform.position;

 

        var effect = PoolSystem.Instance.GetInstance<ParticleSystem>(DestroyedEffect);
        effect.time = 1.0f;
        effect.Play();
        if (destroyed == false)
        {
            //Debug.Log("No se ve la sangre pero entra a hacer la particula");
            effect.transform.position = new Vector3(position.x, position.y - 2, position.z);
        } else
        {
            effect.transform.position = new Vector3(position.x, position.y, position.z);
            
        }

        //blinkTimer = blinkDuration;


        if (HitPlayer != null)
            HitPlayer.PlayRandom();

        if (currentHealth > 0)
            return;


        //the audiosource of the target will get destroyed, so we need to grab a world one and play the clip through it
        // La fuente de audio del objetivo será destruida, por lo que necesitamos tomar una del mundo y reproducir el clip a través de ella.
        //if (HitPlayer != null)
        //{
        //    var source = WorldAudioPool.GetWorldSFXSource();
        //    source.transform.position = position;
        //    source.pitch = HitPlayer.source.pitch;
        //    source.PlayOneShot(HitPlayer.GetRandomClip());
        //}

        //if (DestroyedEffect != null)
        //{
        //var effect = PoolSystem.Instance.GetInstance<ParticleSystem>(DestroyedEffect);
        //effect.time = 0.0f;
        //effect.Play();
        //effect.transform.position = position;
        //}

        // destroyed = true;
        

        

       GameSystem.Instance.TargetDestroyed(pointValue);




    }

    private void Update()
    {
        //blinkTimer -= Time.deltaTime;
        //blinkTimer = blinkDuration;
        //float lerp = Mathf.Clamp01(blinkTimer / blinkDuration);
        //float intensity = (lerp * blinkIntesity);
        //skinnedMeshRenderer.material.color = Color.red * intensity;
        if (destroyed == true)
        {
           
            dieMaxTime -= Time.deltaTime;
            if (dieMaxTime <= 0)
            {
                if (gameObject.CompareTag("Enemy"))
                {
                    // gameObject.SetActive(false);
                    Destroy(gameObject);
                   // Debug.Log("Destruyo al enemigo");
                    dieMaxTime = 0;
                    destroyed = false;
                } else
                if(gameObject.CompareTag("Player"))
                {
                    dieMaxTime = 0;
                    destroyed = false;
                }

            }
        }
     
    }

    private void Die(Vector3 direction)
    {
     
        OnDeath(direction);
      
   
    }

    protected virtual void OnStart()
    {

    }

    protected virtual void OnDeath(Vector3 direction)
    {

    }

    protected virtual void OnDamage(Vector3 direction)
    {

    }

}
