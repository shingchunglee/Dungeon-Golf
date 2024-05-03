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

    // === POWER LEVEL ===
    public float maxVariance;
    public float minVariance;
    public float varianceLevelDuration;
    public EasingFunction varianceEasingFunction;

    public int weight;
}