using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIToggleInstructions : MonoBehaviour
{
    public GameObject instructionsText;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleInstructions();
        }
    }

    public void ToggleInstructions()
    {
        instructionsText.SetActive(!instructionsText.activeSelf);

        if(Time.timeScale == 0)
        {
            Time.timeScale = 1.0f;
        }
        else
        {
            Time.timeScale = 0.0f;
        }
    }
}
