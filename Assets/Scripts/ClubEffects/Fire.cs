
using UnityEngine;

public class Fire : ClubEffects
{
    public override void OnDamageEnemy(EnemyUnit enemy, int damage)
    {
        // enemy.applyStatusEffect(EnemyStatusEffectType.Fire, 3);
        enemy.enemyStatusEffects.Add(EnemyStatusEffectType.Fire, 3);
    }
}