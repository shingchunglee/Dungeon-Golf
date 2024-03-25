using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PowerLevelController : MonoBehaviour
{
  public int minPower;
  public int maxPower;
  public float? selectedPower = null;
  [SerializeField]
  private GameObject powerBarCanvas;
  [SerializeField]
  private Slider powerBarSlider;

  internal void Awake()
  {
    DisablePowerBar();
  }

  internal void ShowPowerBar()
  {
    powerBarCanvas.SetActive(true);
    InitPowerSlider();
  }

  internal void DisablePowerBar()
  {
    powerBarCanvas.SetActive(false);
  }

  private void InitPowerSlider()
  {
    powerBarSlider.maxValue = maxPower;
    powerBarSlider.minValue = minPower;
    selectedPower = null;
    StartCoroutine(Slide());
  }

  IEnumerator Slide()
  {
    powerBarSlider.value = minPower;
    while (selectedPower == null)
    {
      powerBarSlider.value += 1;
      powerBarSlider.value = Mathf.Clamp(powerBarSlider.value, minPower, maxPower);
      if (powerBarSlider.value >= maxPower)
      {
        powerBarSlider.value = minPower;
      }
      yield return new WaitForSeconds(0.005f);
    }
  }

  internal void SelectPowerLevel()
  {
    selectedPower = powerBarSlider.value;
    DisablePowerBar();
  }
}