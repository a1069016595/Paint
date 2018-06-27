using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class SliderText : UIBase
{
    Text text;
    Slider slider;


    void Awake()
    {
        slider = GetComponent<Slider>();
        text = GetChild<Text>("Text");
    }

    void Update()
    {
        text.text = Math.Round(slider.value, 2).ToString();
    }
}
