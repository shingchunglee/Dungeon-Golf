using System;
using UnityEngine;
using UnityEngine.AI;

class Easing
{
  public static float EaseInSine(float x)
  {
    CheckArguments(x);

    return 1 - Mathf.Cos(Mathf.Clamp01(x) * Mathf.PI / 2);
  }

  public static float EaseOutSine(float x)
  {
    CheckArguments(x);

    return Mathf.Sin(Mathf.Clamp01(x) * Mathf.PI / 2);
  }

  public static float EaseOutBounce(float x)
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

  private static void CheckArguments(float x)
  {
    if (x < 0 || x > 1)
    {
      throw new ArgumentException("x must be between 0 and 1");
    }
  }
}