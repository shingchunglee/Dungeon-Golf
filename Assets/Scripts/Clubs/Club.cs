using System;
using Common;
using UnityEngine;

[CreateAssetMenu(fileName = "Club", menuName = "Club", order = 0)]
public class Club : ScriptableObject
{
    public string clubName;

    // === POWER LEVEL ===
    public float maxPower;
    public float minPower;
    public float powerLevelDuration;
    public EasingFunction powerEasingFunction;

    public int weight;
}