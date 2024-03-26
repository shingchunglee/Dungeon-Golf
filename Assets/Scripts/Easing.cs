using System;
using UnityEngine;

public enum EasingFunction
{
  EaseInSine,
  EaseOutSine,
  EaseInOutSine,
  EaseInCubic,
  EaseOutCubic,
  EaseInOutCubic,
  EaseInQuint, EaseOutQuint, EaseInOutQuint,
  EaseInCirc, EaseOutCirc, EaseInOutCirc,
  EaseInQuad, EaseOutQuad, EaseInOutQuad,
  EaseInQuart, EaseOutQuart, EaseInOutQuart,
  EaseInExpo, EaseOutExpo, EaseInOutExpo,
  EaseInBounce, EaseOutBounce, EaseInOutBounce
}

class Easing
{
  public static Func<float, float>
  Factory(EasingFunction easingFunction)
  {
    switch (easingFunction)
    {
      case EasingFunction.EaseInSine:
        return EaseInSine;
      case EasingFunction.EaseOutSine:
        return EaseOutSine;
      case EasingFunction.EaseInOutSine:
        return EaseInOutSine;
      case EasingFunction.EaseInCubic:
        return EaseInCubic;
      case EasingFunction.EaseOutCubic:
        return EaseOutCubic;
      case EasingFunction.EaseInOutCubic:
        return EaseInOutCubic;
      case EasingFunction.EaseInQuint:
        return EaseInQuint;
      case EasingFunction.EaseOutQuint:
        return EaseOutQuint;
      case EasingFunction.EaseInOutQuint:
        return EaseInOutQuint;
      case EasingFunction.EaseInCirc:
        return EaseInCirc;
      case EasingFunction.EaseOutCirc:
        return EaseOutCirc;
      case EasingFunction.EaseInOutCirc:
        return EaseInOutCirc;
      case EasingFunction.EaseInQuad:
        return EaseInQuad;
      case EasingFunction.EaseOutQuad:
        return EaseOutQuad;
      case EasingFunction.EaseInOutQuad:
        return EaseInOutQuad;
      case EasingFunction.EaseInQuart:
        return EaseInQuart;
      case EasingFunction.EaseOutQuart:
        return EaseOutQuart;
      case EasingFunction.EaseInOutQuart:
        return EaseInOutQuart;
      case EasingFunction.EaseInExpo:
        return EaseInExpo;
      case EasingFunction.EaseOutExpo:
        return EaseOutExpo;
      case EasingFunction.EaseInOutExpo:
        return EaseInOutExpo;
      case EasingFunction.EaseInBounce:
        return EaseInBounce;
      case EasingFunction.EaseOutBounce:
        return EaseOutBounce;
      case EasingFunction.EaseInOutBounce:
        return EaseInOutBounce;
      default:
        return EaseInSine;
    }
  }
  private static float EaseInSine(float x)
  {
    CheckArguments(x);

    return 1 - Mathf.Cos(Mathf.Clamp01(x) * Mathf.PI / 2);
  }

  private static float EaseOutSine(float x)
  {
    CheckArguments(x);

    return Mathf.Sin(Mathf.Clamp01(x) * Mathf.PI / 2);
  }

  private static float EaseInOutSine(float x)
  {
    CheckArguments(x);

    return -(Mathf.Cos(Mathf.PI * x) - 1) / 2;
  }

  private static float EaseInCubic(float x)
  {
    CheckArguments(x);

    return x * x * x;
  }

  private static float EaseOutCubic(float x)
  {
    CheckArguments(x);

    return 1 - Mathf.Pow(1f - x, 3f);
  }

  private static float EaseInOutCubic(float x)
  {
    CheckArguments(x);

    return x < 0.5 ? 4 * x * x * x : 1 - Mathf.Pow(-2 * x + 2, 3) / 2;
  }

  private static float EaseInQuint(float x)
  {
    CheckArguments(x);

    return x * x * x * x * x;
  }

  private static float EaseOutQuint(float x)
  {
    CheckArguments(x);

    return 1 - Mathf.Pow(1f - x, 5f);
  }

  private static float EaseInOutQuint(float x)
  {
    CheckArguments(x);

    return x < 0.5 ? 16 * x * x * x * x * x : 1 - Mathf.Pow(-2 * x + 2, 5) / 2;
  }

  private static float EaseInCirc(float x)
  {
    CheckArguments(x);

    return 1 - Mathf.Sqrt(1 - Mathf.Pow(x, 2));
  }

  private static float EaseOutCirc(float x)
  {
    CheckArguments(x);

    return Mathf.Sqrt(1 - Mathf.Pow(x - 1, 2));
  }

  private static float EaseInOutCirc(float x)
  {
    CheckArguments(x);

    return x < 0.5
  ? (1 - Mathf.Sqrt(1 - Mathf.Pow(2 * x, 2))) / 2
  : (Mathf.Sqrt(1 - Mathf.Pow(-2 * x + 2, 2)) + 1) / 2;
  }

  private static float EaseInQuad(float x)
  {
    CheckArguments(x);

    return x * x;
  }

  private static float EaseOutQuad(float x)
  {
    CheckArguments(x);

    return 1 - (1 - x) * (1 - x);
  }

  private static float EaseInOutQuad(float x)
  {
    CheckArguments(x);

    return x < 0.5 ? 2 * x * x : 1 - Mathf.Pow(-2 * x + 2, 2) / 2;
  }

  private static float EaseInQuart(float x)
  {
    CheckArguments(x);

    return x * x * x * x;
  }

  private static float EaseOutQuart(float x)
  {
    CheckArguments(x);

    return 1 - Mathf.Pow(1 - x, 4);
  }

  private static float EaseInOutQuart(float x)
  {
    CheckArguments(x);

    return x < 0.5 ? 8 * x * x * x * x : 1 - Mathf.Pow(-2 * x + 2, 4) / 2;
  }

  private static float EaseInExpo(float x)
  {
    CheckArguments(x);

    return x == 0 ? 0 : Mathf.Pow(2, 10 * x - 10);
  }

  private static float EaseOutExpo(float x)
  {
    CheckArguments(x);

    return x == 1 ? 1 : 1 - Mathf.Pow(2, -10 * x);
  }

  private static float EaseInOutExpo(float x)
  {
    CheckArguments(x);

    return x == 0
      ? 0
      : x == 1
      ? 1
      : x < 0.5 ? Mathf.Pow(2, 20 * x - 10) / 2
      : (2 - Mathf.Pow(2, -20 * x + 10)) / 2;
  }

  private static float EaseInBounce(float x)
  {
    CheckArguments(x);

    return 1 - EaseOutBounce(1 - x);
  }

  private static float EaseOutBounce(float x)
  {
    CheckArguments(x);

    const float n1 = 7.5625f;
    const float d1 = 2.75f;

    if (x < 1 / d1)
    {
      return n1 * x * x;
    }
    else if (x < 2 / d1)
    {
      return n1 * (x -= 1.5f / d1) * x + 0.75f;
    }
    else if (x < 2.5 / d1)
    {
      return n1 * (x -= 2.25f / d1) * x + 0.9375f;
    }
    else
    {
      return n1 * (2.625f / d1) * x + 0.984375f;
    }
  }

  private static float EaseInOutBounce(float x)
  {
    CheckArguments(x);

    return x < 0.5
  ? (1 - EaseOutBounce(1 - 2 * x)) / 2
  : (1 + EaseOutBounce(2 * x - 1)) / 2;
  }

  private static void CheckArguments(float x)
  {
    if (x < 0 || x > 1)
    {
      throw new ArgumentException("x must be between 0 and 1");
    }
  }
}