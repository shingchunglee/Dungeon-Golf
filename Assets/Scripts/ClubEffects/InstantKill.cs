using UnityEngine;

public class InstantKill : ClubEffects
{
    public InstantKill()
    {
        statusEffectType = PlayerStatusEffect.StatusEffectType.INSTAKILL;
    }
    private const float instantKillChance = 0.1f;
    private const int highDamageAmount = 10; // Define the high damage amount

    public override void OnDamageEnemy(EnemyUnit enemy, int damage)
    {
        if (Random.value <= instantKillChance)
        {
            enemy.CurrentHP = 0; // Set the enemy's health to zero
        }
        else
        {
            enemy.TakeDamage(highDamageAmount); // Deal high damage
        }
        enemy.CheckIfDead(); // Check if the enemy should be killed
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
