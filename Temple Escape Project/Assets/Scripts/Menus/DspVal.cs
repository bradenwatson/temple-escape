using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DspVal : MonoBehaviour
{
    public Slider Slider;
    public Text text;
    public int multVal;
    public void dsp(float val)
    {
        float frac = (val - Slider.minValue) / (val - Slider.maxValue - Slider.minValue);
        text.text = Mathf.FloorToInt(frac * multVal).ToString();
    }
}
