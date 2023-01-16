using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DebugDrawLine : MonoBehaviour
{
  

   private void OnDrawGizmos()
    {
      // Debug.Log("Entra acá");
       
        Debug.DrawLine(transform.position, transform.position + transform.forward * 50, Color.green);
    }
}
