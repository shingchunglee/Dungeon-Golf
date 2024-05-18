
public class HealPlayer : ClubEffects
{
    public override void AfterPlayerMove()
    {
        PlayerManager.Instance.RestoreHealth(5);
    }

}