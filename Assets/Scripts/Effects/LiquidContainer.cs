// This is based on the great article by Minions Art (https://www.patreon.com/posts/quick-game-art-18245226)
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidContainer : MonoBehaviour
{
    public float MaxWobble = 0.03f;
    public float WobbleSpeed = 1f;
    public float Recovery = 1f;
    
    Renderer mrenderer;
    Vector3 previousPosition;
    Vector3 velocity;
    Vector3 lastRotation;  
    Vector3 angularVelocity;
    float wobbleAmountX;
    float wobbleAmountZ;
    float wobbleAmountToAddX;
    float wobbleAmountToAddZ;
    float pulse;
    float time = 0.5f;

    Material material;

    int liquidRotationId;
    int fillAmountId;
    
    // Use this for initialization
    void Awake()
    {
        mrenderer = GetComponent<Renderer>();
        material = mrenderer.material;

        liquidRotationId = Shader.PropertyToID("_LiquidRotation");
        fillAmountId = Shader.PropertyToID("_FillAmount");
    }

    public void ChangeLiquidAmount(float liquidAmount)
    {
        material.SetFloat(fillAmountId, liquidAmount);
    }
    
    private void Update()
    {
        time += Time.deltaTime;
        // decrease wobble over time
        wobbleAmountToAddX = Mathf.Lerp(wobbleAmountToAddX, 0, Time.deltaTime * (Recovery));
        wobbleAmountToAddZ = Mathf.Lerp(wobbleAmountToAddZ, 0, Time.deltaTime * (Recovery));

        // make a sine wave of the decreasing wobble
        pulse = 2 * Mathf.PI * WobbleSpeed;
        wobbleAmountX = wobbleAmountToAddX * Mathf.Sin(pulse * time);
        wobbleAmountZ = wobbleAmountToAddZ * Mathf.Sin(pulse * time);
        
        Matrix4x4 rotation = Matrix4x4.Rotate( Quaternion.AngleAxis(wobbleAmountZ, Vector3.right) * Quaternion.AngleAxis(wobbleAmountX, Vector3.forward));

        // send it to the shader
        material.SetMatrix(liquidRotationId, rotation);
        
        // velocity
        velocity = (previousPosition - transform.position) / Time.deltaTime;
        angularVelocity = transform.rotation.eulerAngles - lastRotation;


        // add clamped velocity to wobble
        wobbleAmountToAddX += Mathf.Clamp((velocity.x + (angularVelocity.z * 0.2f)) * MaxWobble, -MaxWobble, MaxWobble);
        wobbleAmountToAddZ += Mathf.Clamp((velocity.z + (angularVelocity.x * 0.2f)) * MaxWobble, -MaxWobble, MaxWobble);

        // keep last position
        previousPosition = transform.position;
        lastRotation = transform.rotation.eulerAngles;
    }
}
