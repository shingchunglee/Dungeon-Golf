
using UnityEngine;

public class Freezing : ClubEffects
{
    public override void OnDamageEnemy(EnemyUnit enemy, int damage)
    {
        enemy.applyStatusEffect(EnemyStatusEffectType.Frozen, 1);
    }
}