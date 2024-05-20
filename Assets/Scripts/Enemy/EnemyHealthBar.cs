using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Slider HealthSlider;

    public void UpdateHealthBar(float currentValue, float maxValue)
    {
        HealthSlider.value = currentValue / maxValue;
    }
}