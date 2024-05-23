using UnityEngine;

class PlayerVampirism : PlayerStatusEffect.StatusEffect
{
    public PlayerVampirism(int turns)
    {
        type = PlayerStatusEffect.StatusEffectType.VAMPIRISM;
        this.turns = turns;
        this.isInfinite = true;
    }
}