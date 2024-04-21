using System.Collections;
using Common;
using UnityEngine;
using UnityEngine.UI;

public class VarianceLevelController : MonoBehaviour
{
  public float minVariance;
  public float maxVariance;
  public float duration;
  public EasingFunction easingFunction;
  public float? selectedVariance = null;
  [SerializeField]
  private GameObject varianceBarCanvas;
  [SerializeField]
  private Slider varianceBarSlider;

  internal void Awake()
  {
    DisableVarianceBar();
    InventoryController.OnClubChanged += OnClubChanged;
  }

  private void OnClubChanged(Club club)
  {
    minVariance = club.minVariance;
    maxVariance = club.maxVariance;
    duration = club.varianceLevelDuration;
    easingFunction = club.varianceEasingFunction;
  }

  internal void ShowVarianceBar()
  {
    varianceBarCanvas.SetActive(true);
    InitVarianceSlider();
  }

  internal void DisableVarianceBar()
  {
    varianceBarCanvas.SetActive(false);
  }

  private void InitVarianceSlider()
  {
    varianceBarSlider.minValue = 0f;
    varianceBarSlider.maxValue = 1f;
    // TODO update values based on club
    // duration = 1f;
    // easingFunction = EasingFunction.EaseOutCubic;

    selectedVariance = null;
    StartCoroutine(Slide());
  }

  IEnumerator Slide()
  {
    varianceBarSlider.value = 0;
    float time = 0;

    while (selectedVariance == null)
    {
      time += Time.deltaTime;
      float delta = Mathf.Clamp01(time / duration);
      float t = Easing.Factory(easingFunction)(delta);

      float value = Mathf.Lerp(0f, 1f, t);
      varianceBarSlider.value = value;

      if (delta >= 0.99f)
      {
        varianceBarSlider.value = 0;
        time = 0;
      }

      yield return null;
    }
  }

  internal void SelectVarianceLevel()
  {
    SetVariance(varianceBarSlider.value * (maxVariance - minVariance) + minVariance);
    DisableVarianceBar();
  }

  public void SetVariance(float variance)
  {
    selectedVariance = variance;
  }
}