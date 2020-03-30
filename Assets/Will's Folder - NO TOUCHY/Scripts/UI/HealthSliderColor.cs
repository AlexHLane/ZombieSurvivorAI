using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSliderColor : MonoBehaviour
{
    private Slider healthSlider;
    private Image healthImageColor;


    // Start is called before the first frame update
    void Start()
    {
        healthSlider = GetComponent<Slider>();
        healthImageColor = GetComponentInChildren<Image>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnGUI()
    {
        float curValue = 1 - (healthSlider.maxValue - healthSlider.value) / healthSlider.maxValue;

        if(curValue >= 0.75f)
        {
            healthImageColor.color = Color.green;
        }
        else if(curValue >= 0.25f)
        {
            healthImageColor.color = Color.yellow;
        }
        else
        {
            healthImageColor.color = Color.red;
        }
    }
}
