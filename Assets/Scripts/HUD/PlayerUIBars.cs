using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIBars : MonoBehaviour
{
    [SerializeField] private Slider HealthSlider;
    [SerializeField] private Slider EXPSlider;

    public TextMeshProUGUI HPText;
    public TextMeshProUGUI EXPText;

    public void UpdateHealthBar(float currentValue, float maxValue)
    {
        HealthSlider.value = currentValue / maxValue;
        if (HPText.text != null)
        {
            HPText.text = $"HP: {currentValue}/{maxValue}";
        }
    }

    public void UpdateEXPBar(float currentValue, float maxValue)
    {
        EXPSlider.value = currentValue / maxValue;
        if (EXPText != null)
        {
            EXPText.text = $"EXP: {currentValue}/{maxValue}";
        }
    }

    void Update()
    {

    }
}
