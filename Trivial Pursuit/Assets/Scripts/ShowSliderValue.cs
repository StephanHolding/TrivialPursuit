using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowSliderValue : MonoBehaviour {

    public Slider slider;
    public Text valueText;
    public bool showValue;

    private void Update()
    {
        if (showValue)
        {
            int rounded = Mathf.RoundToInt(slider.value);
            rounded += 100;
            valueText.text = rounded.ToString();
        }
    }
}
