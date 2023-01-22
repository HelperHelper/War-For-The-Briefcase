using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WimGameGhosTwom : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            WimPanel.Instance.WimCementary();
            GameState.Instance.UnlockCementary();
        }
    }
}
