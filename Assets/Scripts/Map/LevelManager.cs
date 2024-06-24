using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public bool isProceduralGen = true;

    public int shotsTakenOnLevel { get; private set; } = 0;

    public int EXPRewardEndLevel = 50;

    [Header("Set Par in Procedural Generation Script if Procgen")]
    [Tooltip("Cool par tooltip here!")]
    public int par = 99;
}
