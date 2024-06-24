using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Slider HealthSlider;
    public bool displayHPBarAtFull = false;

    public void UpdateHealthBar(float currentValue, float maxValue)
    {
        float newValue = currentValue / maxValue;

        if (newValue == 1)
        {
            HealthSlider.gameObject.SetActive(true);
            HealthSlider.value = newValue;
            HealthSlider.gameObject.SetActive(false);
        }
        else
        {
            HealthSlider.gameObject.SetActive(true);
            HealthSlider.value = newValue;
        }
    }
}