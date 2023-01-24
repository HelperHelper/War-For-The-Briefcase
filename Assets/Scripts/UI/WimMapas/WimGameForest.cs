using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WimGameForest : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            WimPanel.Instance.WimForest();
            GameSystem.Instance.StopTimer();
            GameSystem.Instance.FinishRun();
        }
    }
}

