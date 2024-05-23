
public class HealPlayer : ClubEffects
{
    public HealPlayer()
    {
        statusEffectType = PlayerStatusEffect.StatusEffectType.HEALING;
    }
    public override void AfterPlayerMove()
    {
        PlayerManager.Instance.RestoreHealth(5);
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