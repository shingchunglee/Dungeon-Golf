using System;

public enum ClubEffectsType
{
    HealPlayer, // heal player for 5 hp after move
    Vampirism, // restore 50% of damage dealt
    Freezing, // freeze enemy for 1 turn
    Fire, // Deal 5 ticking damage per turn before enemies move
    Stun,
    InstantKill,
    Curse,
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
            case ClubEffectsType.Fire:
                return new Fire();
            case ClubEffectsType.Stun:
                return new Stun();
            case ClubEffectsType.InstantKill: // Add this line
                return new InstantKill();
            case ClubEffectsType.Curse: // Add this line
                return new Curse();
            default:
                return new ClubEffects();
        }
    }
}

public class ClubEffects
{
    public virtual void OnDamageEnemy(EnemyUnit enemy, int damage) { }

    public virtual void OnClubChanged(Club club) { }

    public virtual void OnClubRemoved(Club club) { }
    public virtual void AfterPlayerMove() { }
}
