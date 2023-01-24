using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Levels : MonoBehaviour
{

    public Button[ ] levelsButton;

    private void Start()
    {
        int levelsEscenas = PlayerPrefs.GetInt("levescen", 1);

        for (int i = 0; i < levelsButton.Length; i++)
        {
            if (i + 1 > levelsEscenas)
            {
                levelsButton[i].interactable = false; 
            }
        }
    }




}
