
using System;
using UnityEngine;

public class Vampirism : ClubEffects
{
    public Vampirism()
    {
        statusEffectType = PlayerStatusEffect.StatusEffectType.VAMPIRISM;
    }
    public override void OnDamageEnemy(EnemyUnit enemy, int damage)
    {
        if (UnityEngine.Random.Range(0, 2) == 0)
        {
            PlayerManager.Instance.RestoreHealth(1);
        }

        // PlayerManager.Instance.RestoreHealth(1);
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