using UnityEngine;

class PlayerFreezing : PlayerStatusEffect.StatusEffect
{
    public PlayerFreezing(int turns)
    {
        type = PlayerStatusEffect.StatusEffectType.FREEZING;
        this.turns = turns;
        this.isInfinite = true;
    }
}