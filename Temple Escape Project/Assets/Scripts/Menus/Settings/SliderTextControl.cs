using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderTextControl : MonoBehaviour
{
    [Header("Slider & Text")]
    public Slider selectedSlider;
    public int multiplierValue = 100;

    // Update slider text with new value.
    public void UpdateText(float value)
    {
        float fraction = (value - selectedSlider.minValue) /
            (selectedSlider.maxValue - selectedSlider.minValue);
    }
}
