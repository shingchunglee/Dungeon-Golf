using System.Collections;
using Common;
using UnityEngine;
using UnityEngine.UI;

public class PowerLevelController : MonoBehaviour
{
  public int minPower;
  public int maxPower;
  public float duration;
  public EasingFunction easingFunction;
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
    powerBarSlider.minValue = 0f;
    powerBarSlider.maxValue = 1f;
    // TODO update values based on club
    // duration = 1f;
    easingFunction = EasingFunction.EaseOutCubic;

    selectedPower = null;
    StartCoroutine(Slide());
  }

  IEnumerator Slide()
  {
    powerBarSlider.value = 0;
    float time = 0;

    while (selectedPower == null)
    {
      time += Time.deltaTime;
      float delta = Mathf.Clamp01(time / duration);
      float t = Easing.Factory(easingFunction)(delta);

      float value = Mathf.Lerp(0f, 1f, t);
      powerBarSlider.value = value;

      if (delta >= 0.99f)
      {
        powerBarSlider.value = 0;
        time = 0;
      }

      yield return null;
    }
  }

  internal void SelectPowerLevel()
  {
    selectedPower = powerBarSlider.value * (maxPower - minPower) + minPower;
    DisablePowerBar();
  }
}