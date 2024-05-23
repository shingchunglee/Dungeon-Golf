
using UnityEngine;

public class Fire : ClubEffects
{
    public override void OnDamageEnemy(EnemyUnit enemy, int damage)
    {
        var fire = enemy.enemyStatusEffects.Add(EnemyStatusEffectType.Fire, 3);
        fire.OnTakeTurn = (enemy) => { enemy.TakeDamage(5); };
        fire.priority.onTakeTurn = 1;
    }

    public override void OnClubChanged(Club club)
    {
        PlayerManager.Instance.statusEffect.Add(PlayerStatusEffect.StatusEffectType.FLAME, 0);
    }
    public override void OnClubRemoved(Club club)
    {
        PlayerManager.Instance.statusEffect.Remove(PlayerStatusEffect.StatusEffectType.FLAME);
    }
}