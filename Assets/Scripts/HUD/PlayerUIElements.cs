using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIElements : MonoBehaviour
{
    [SerializeField] private Slider HealthSlider;
    [SerializeField] private Slider EXPSliderHUD;
    private LevelUpUI levelUpUI;

    [SerializeField] private TextMeshProUGUI HPTextHUD;
    [SerializeField] private TextMeshProUGUI LevelTextHUD;

    [SerializeField] private TextMeshProUGUI EXPTextMenu;
    [SerializeField] private TextMeshProUGUI LevelTextMenu;
    [SerializeField] private TextMeshProUGUI basePowerLevelTextMenu;
    [SerializeField] private TextMeshProUGUI clubPowerLevelTextMenu;
    [SerializeField] private TextMeshProUGUI damageTotalTextMenu;
    [SerializeField] private TextMeshProUGUI damageTotalTextHUD;
    [SerializeField] private TextMeshProUGUI clubEffectTextMenu;
    [SerializeField] private Slider EXPSliderMenu;


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

    public void UpdateClubUI(Club club)
    {
        UpdateClubEffectTextInMenu("Text here.");
        UpdateDamageTotalEverywhere(PlayerManager.Instance.currentDamage.ToString());
        UpdateClubPowerLevelTextInMenu(club.damage.ToString());
    }

    private void UpdateClubEffectTextInMenu(string text)
    {
        if (clubEffectTextMenu != null)
        {
            clubEffectTextMenu.text = text;
        }
    }

    public void UpdateDamageTotalEverywhere(string text)
    {
        if (damageTotalTextMenu != null)
        {
            damageTotalTextMenu.text = text;
        }

        if (damageTotalTextHUD != null)
        {
            damageTotalTextHUD.text = text;
        }
    }

    private void UpdateClubPowerLevelTextInMenu(string text)
    {
        if (clubPowerLevelTextMenu != null)
        {
            clubPowerLevelTextMenu.text = text;
        }
    }

    public void UpdateBasePowerLevelTextInMenu(string text)
    {
        if (basePowerLevelTextMenu != null)
        {
            basePowerLevelTextMenu.text = text;
        }
    }

    private void UpdateEXPInMenu(string text)
    {
        if (EXPTextMenu != null)
        {
            EXPTextMenu.text = text;
        }
    }

    public void UpdateHealthBar(float currentValue, float maxValue)
    {
        HealthSlider.value = currentValue / maxValue;
        if (HPTextHUD.text != null)
        {
            HPTextHUD.text = $"HP: {currentValue}/{maxValue}";
        }
    }

    public void UpdateEXPUI(float currentValue, float nextLevelValue)
    {
        EXPSliderHUD.value = currentValue / nextLevelValue;
        EXPSliderMenu.value = currentValue / nextLevelValue;

        UpdateEXPInMenu($"{currentValue}/{nextLevelValue}");
    }

    private int previousLevelNumber = 1;

    public void UpdateLevelTextEverywhere(int value)
    {
        LevelTextHUD.text = value.ToString();

        if (LevelTextMenu != null)
        {
            LevelTextMenu.text = value.ToString();
        }

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
            LevelTextHUD.color = textColourFlash;
            yield return new WaitForSeconds(0.1f);
            LevelTextHUD.color = textColourBase;

        }
    }
}
