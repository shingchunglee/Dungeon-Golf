using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy")]
public class EnemyData : ScriptableObject
{
    public string enemyName;
    public GameObject enemyPrefab;
    public int baseHP;
    public int baseAttack;
}

