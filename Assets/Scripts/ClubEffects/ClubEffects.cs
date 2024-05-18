using System;

public enum ClubEffectsType
{
    HealPlayer, // heal player for 5 hp after move
    Vampirism, // restore 50% of damage dealt
    Freezing, // freeze enemy for 1 turn
}

public class ClubEffectsFactory
{
    public static ClubEffects Create(ClubEffectsType type)
    {
        switch (type)
        {
            case ClubEffectsType.HealPlayer:
                return new HealPlayer();
            case ClubEffectsType.Vampirism:
                return new Vampirism();
            case ClubEffectsType.Freezing:
                return new Freezing();
            default:
                return new ClubEffects();
        }
    }
}

public class ClubEffects
{
    public virtual void OnDamageEnemy(EnemyUnit enemy, int damage) { }
    public virtual void AfterPlayerMove() { }
}
