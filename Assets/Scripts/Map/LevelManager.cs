using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public bool isProceduralGen = true;

    public int shotsTakenOnLevel { get; private set; } = 0;

    [Header("Set Par in Procedural Generation Script if Procgen")]
    [Tooltip("Do it!")]
    public int par = 99;
}
