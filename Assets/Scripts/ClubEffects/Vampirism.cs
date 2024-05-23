
using UnityEngine;

public class Vampirism : ClubEffects
{
    public Vampirism()
    {
        statusEffectType = PlayerStatusEffect.StatusEffectType.VAMPIRISM;
    }
    public override void OnDamageEnemy(EnemyUnit enemy, int damage)
    {
        PlayerManager.Instance.RestoreHealth(Mathf.CeilToInt(damage * 0.2f));
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