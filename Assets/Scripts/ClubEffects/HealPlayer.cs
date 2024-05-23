
public class HealPlayer : ClubEffects
{
    public override void AfterPlayerMove()
    {
        PlayerManager.Instance.RestoreHealth(5);
    }

    public override void OnClubChanged(Club club)
    {
        PlayerManager.Instance.statusEffect.Add(PlayerStatusEffect.StatusEffectType.HEALING, 0);
    }
    public override void OnClubRemoved(Club club)
    {
        PlayerManager.Instance.statusEffect.Remove(PlayerStatusEffect.StatusEffectType.HEALING);
    }
}