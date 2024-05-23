
using UnityEngine;

public class Vampirism : ClubEffects
{
    public override void OnDamageEnemy(EnemyUnit enemy, int damage)
    {
        PlayerManager.Instance.RestoreHealth(Mathf.FloorToInt(damage / 2));
    }

    public override void OnClubChanged(Club club)
    {
        PlayerManager.Instance.statusEffect.Add(PlayerStatusEffect.StatusEffectType.VAMPIRISM, 0);
    }
    public override void OnClubRemoved(Club club)
    {
        PlayerManager.Instance.statusEffect.Remove(PlayerStatusEffect.StatusEffectType.VAMPIRISM);
    }
}