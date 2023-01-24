using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnlockLevels : MonoBehaviour
{

    public Button[] levelsButtons;

    private void Start()
    {
        int nivel = PlayerPrefs.GetInt("nivel", 2);

        for ( int i = 0; i < levelsButtons.Length; i++)
        {
            if (i + 2 > nivel)
            {
                levelsButtons[i].interactable = false;
            }
        }
    }

}
