using System;

public enum ClubEffectsType
{
    HealPlayer,
}

public class ClubEffectsFactory
{
    public static ClubEffects Create(ClubEffectsType type)
    {
        switch (type)
        {
            case ClubEffectsType.HealPlayer:
                return new HealPlayer();
            default:
                return new ClubEffects();
        }
    }
}

public class ClubEffects
{
    public virtual void OnDamageEnemy(EnemyUnit enemy) { }
    public virtual void AfterPlayerMove() { }
}
