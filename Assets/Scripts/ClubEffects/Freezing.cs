using UnityEngine;

public class Freezing : ClubEffects
{
    public Freezing()
    {
        statusEffectType = PlayerStatusEffect.StatusEffectType.FREEZING;
    }
    public override void OnDamageEnemy(EnemyUnit enemy, int damage)
    {
        // enemy.applyStatusEffect(EnemyStatusEffectType.Frozen, 1);
        var frozen = enemy.enemyStatusEffects.Add(EnemyStatusEffectType.Frozen, 1);
        frozen.OnTakeTurn = (enemy) => { enemy.skipTurn = true; };
        frozen.priority.onTakeTurn = -1;
    }

    public override void OnClubChanged(Club club)
    {
        PlayerManager.Instance.statusEffect.Add(statusEffectType, 0);
    }
    public override void OnClubRemoved(Club club)
    {
        PlayerManager.Instance.statusEffect.Remove(statusEffectType);
    }
}