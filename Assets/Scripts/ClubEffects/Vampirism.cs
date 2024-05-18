
using UnityEngine;

public class Vampirism : ClubEffects
{
    public override void OnDamageEnemy(EnemyUnit enemy, int damage)
    {
        PlayerManager.Instance.RestoreHealth(Mathf.FloorToInt(damage / 2));
    }
}