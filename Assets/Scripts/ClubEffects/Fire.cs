
using UnityEngine;

public class Fire : ClubEffects
{
    public Fire()
    {
        statusEffectType = PlayerStatusEffect.StatusEffectType.FLAME;
    }
    public override void OnDamageEnemy(EnemyUnit enemy, int damage)
    {
        var fire = enemy.enemyStatusEffects.Add(EnemyStatusEffectType.Fire, 3);
        fire.OnTakeTurn = (enemy) => { enemy.TakeDamage(5); };
        fire.priority.onTakeTurn = 1;
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