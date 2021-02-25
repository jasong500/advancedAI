using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISlidersWidget : MonoBehaviour
{
    public Slider avoidanceSlider = null;
    public Slider alignmentSlider = null;
    public Slider cohesionSlider = null;
    public CompositeBehavior myCompBehavior;

    private void Start()
    {
        avoidanceSlider.maxValue = 100.0f;
        avoidanceSlider.minValue = 0.0f;
        alignmentSlider.maxValue = 100.0f;
        alignmentSlider.minValue = 0.0f;
        cohesionSlider.maxValue = 100.0f;
        cohesionSlider.minValue = 0.0f;

        cohesionSlider.value = 4.0f;
        alignmentSlider.value = 1.0f;
        avoidanceSlider.value = 2.0f;
    }

    private void Update()
    {
        float avoid = avoidanceSlider.value;
        float align = alignmentSlider.value;
        float cohesion = cohesionSlider.value;

        myCompBehavior.setWeights(cohesion, align, avoid);
    }

    //public void Setup()
    //{
    //    float sep = separationSlider.value;

    //    alignmentSlider.value = flocking.instance.alignmentWeight;
    //    alignmentSlider.onValueChanged.AddListener((value) => flocking.instance.alignmentWeight = value);

    //    cohesionSlider.value = flocking.instance.cohesionWeight;
    //    cohesionSlider.onValueChanged.AddListener((value) => flocking.instance.cohesionWeight = value);
    //}
}
