
using UnityEngine;

public class Freezing : ClubEffects
{
    public override void OnDamageEnemy(EnemyUnit enemy, int damage)
    {
        // enemy.applyStatusEffect(EnemyStatusEffectType.Frozen, 1);
        var frozen = enemy.enemyStatusEffects.Add(EnemyStatusEffectType.Frozen, 1);
        frozen.OnTakeTurn = (enemy) => { enemy.skipTurn = true; };
        frozen.priority.onTakeTurn = -1;
    }
}