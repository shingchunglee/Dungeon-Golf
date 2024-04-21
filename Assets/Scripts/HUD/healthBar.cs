using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthBar : MonoBehaviour
{
    public Slider slider;
    

    public void SetMaxHealth(float maxHP)
    {
        slider.maxValue = maxHP;
        slider.value  =maxHP;
    }

    public void SetHealth(float currentHP)
    {
        slider.value = currentHP;
    }

    
}
