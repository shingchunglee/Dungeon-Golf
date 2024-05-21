using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIElements : MonoBehaviour
{
    [SerializeField] private Slider HealthSlider;
    [SerializeField] private Slider EXPSlider;
    private LevelUpUI levelUpUI;

    public TextMeshProUGUI HPText;
    public TextMeshProUGUI EXPText;
    public TextMeshProUGUI LevelText;

    public Color textColourBase;
    public Color textColourFlash;

    private void Awake()
    {
        levelUpUI = transform.Find("Level Up Panel").GetComponent<LevelUpUI>();

        Init();
    }

    public void Init()
    {
        previousLevelNumber = 1;
    }

    public void PlayLevelUpAnimation()
    {
        levelUpUI.LevelUpAnimation();
    }

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
    }

    private int previousLevelNumber = 1;

    public void UpdateLevelText(int value)
    {
        LevelText.text = value.ToString();

        if (value != previousLevelNumber)
        {
            previousLevelNumber = value;
            StartCoroutine(FlashText());
        }
    }

    private IEnumerator FlashText()
    {
        for (int i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(0.1f);
            LevelText.color = textColourFlash;
            yield return new WaitForSeconds(0.1f);
            LevelText.color = textColourBase;

        }
    }
}
