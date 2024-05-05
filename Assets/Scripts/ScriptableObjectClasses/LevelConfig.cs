using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New LevelConfig", menuName = "Level Configuration")]
public class LevelConfig : ScriptableObject
{
    public List<EnemyData> enemies;  // List of enemies to spawn in the levels that use this configuration
}


