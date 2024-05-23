using UnityEngine;

public class Curse : ClubEffects
{
    public override void OnDamageEnemy(EnemyUnit enemy, int damage)
    {
        var curse = enemy.enemyStatusEffects.Add(EnemyStatusEffectType.Curse, 1);
        curse.OnTakeTurn = (enemy) =>
        {
            enemy.attackDamage = Mathf.Max(1, enemy.attackDamage - 2); // Reduce attack damage by 1, but not below 1
        };
        curse.priority.onTakeTurn = -1;
    }

    public override void OnClubChanged(Club club)
    {
        PlayerManager.Instance.statusEffect.Add(PlayerStatusEffect.StatusEffectType.CURSE, 0);
    }
    public override void OnClubRemoved(Club club)
    {
        PlayerManager.Instance.statusEffect.Remove(PlayerStatusEffect.StatusEffectType.CURSE);
    }
}