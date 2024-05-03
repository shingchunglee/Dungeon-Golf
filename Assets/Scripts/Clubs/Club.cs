using Common;
using UnityEngine;

[CreateAssetMenu(fileName = "Club", menuName = "Club", order = 0)]
public class Club : ScriptableObject
{
    public string clubName;
    //club sprite?
    //club description?

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
    public float damage;

    public int weight;
    public int appearsAfter = 0;

    public Sprite sprite;
}