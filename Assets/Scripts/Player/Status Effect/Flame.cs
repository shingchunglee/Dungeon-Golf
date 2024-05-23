using UnityEngine;

class PlayerFlame : PlayerStatusEffect.StatusEffect
{
    public PlayerFlame(int turns)
    {
        type = PlayerStatusEffect.StatusEffectType.FLAME;
        this.turns = turns;
        this.isInfinite = true;
    }
}