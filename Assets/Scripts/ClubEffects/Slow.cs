using UnityEngine;

public class Slow : ClubEffects
{
    public override void OnDamageEnemy(EnemyUnit enemy, int damage)
    {
        var slowEffect = enemy.enemyStatusEffects.Add(EnemyStatusEffectType.Slow, 3); // Slow for 3 turns
        slowEffect.OnTakeTurn = (enemy) => { enemy.ReduceMoveSpeed(0.5f); }; // Reduce speed by 50%
        slowEffect.priority.onTakeTurn = -1;
    }
}

