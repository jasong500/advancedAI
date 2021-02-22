using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISlidersWidget : MonoBehaviour
{
    public Slider separationSlider = null;
    public Slider alignmentSlider = null;
    public Slider cohesionSlider = null;

    public void Setup()
    {
        separationSlider.value = flocking.instance.separationWeight;
        separationSlider.onValueChanged.AddListener((value) => flocking.instance.separationWeight = value);

        alignmentSlider.value = flocking.instance.alignmentWeight;
        alignmentSlider.onValueChanged.AddListener((value) => flocking.instance.alignmentWeight = value);

        cohesionSlider.value = flocking.instance.cohesionWeight;
        cohesionSlider.onValueChanged.AddListener((value) => flocking.instance.cohesionWeight = value);
    }
}
