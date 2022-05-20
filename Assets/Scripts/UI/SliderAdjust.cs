using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderAdjust : MonoBehaviour
{
    public string text;
    public int value;
    public TMP_Text textDisplay;
    public Slider slider;
    
    // Start is called before the first frame update
    void Start()
    {
        slider.value = value;
        textDisplay.text = text;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
