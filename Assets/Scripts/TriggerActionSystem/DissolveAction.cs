using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveAction : GameAction
{
    public float DissolveEffectTime = 2;
    public AnimationCurve FadeIn;

    public GameAction[] FinishedAction;

    //ParticleSystem particleSystem;
    float timer = 0;

    Renderer[] renderers;
    MaterialPropertyBlock propertyBlock;

    int cutoffProperty;

    void Start()
    {
        cutoffProperty = Shader.PropertyToID("_Cutoff");
        renderers = GetComponentsInChildren<Renderer>();

        propertyBlock = new MaterialPropertyBlock();

       // particleSystem = GetComponentInChildren<ParticleSystem>();

        //var main = m_ParticleSystem.main;
        //main.duration = DissolveEffectTime;

        //Desactivar para evitar que la función Update sea llamada. Al ser llamada por el GameTrigger se reactivará
        enabled = false;
    }

    void Update()
    {
        timer += Time.deltaTime;

        float value = FadeIn.Evaluate(Mathf.InverseLerp(0, DissolveEffectTime, timer));

        propertyBlock.SetFloat(cutoffProperty, value);
        foreach (var r in renderers)
        {
            r.SetPropertyBlock(propertyBlock);
        }

        if (timer > DissolveEffectTime)
        {
            foreach (var gameAction in FinishedAction)
            {
                gameAction.Activated();
            }

            Destroy(gameObject);
        }
    }

    public override void Activated()
    {
        enabled = true;
       // m_ParticleSystem.Play();
    }
}
