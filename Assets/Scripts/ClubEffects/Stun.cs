
using UnityEngine;

public class Stun : ClubEffects
{
    public override void OnDamageEnemy(EnemyUnit enemy, int damage)
    {
        // enemy.applyStatusEffect(EnemyStatusEffectType.Frozen, 1);
        var frozen = enemy.enemyStatusEffects.Add(EnemyStatusEffectType.Stun, 2);
        frozen.OnTakeTurn = (enemy) => { enemy.skipTurn = true; };
        frozen.priority.onTakeTurn = -1;
    }
}


