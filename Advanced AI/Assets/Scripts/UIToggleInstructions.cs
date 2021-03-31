using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIToggleInstructions : MonoBehaviour
{
    public GameObject instructionsText;

    public void ToggleInstructions()
    {
        instructionsText.SetActive(!instructionsText.activeSelf);
    }
}
