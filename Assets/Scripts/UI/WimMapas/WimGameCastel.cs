using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WimGameCastel : MonoBehaviour
{


        private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            WimPanel.Instance.WimCastel();
            GameSystem.Instance.StopTimer();
            GameSystem.Instance.FinishRun();

        }
    }
}
