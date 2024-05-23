using UnityEngine;

class PlayerHealing : PlayerStatusEffect.StatusEffect
{
    public PlayerHealing(int turns)
    {
        type = PlayerStatusEffect.StatusEffectType.HEALING;
        this.turns = turns;
        this.isInfinite = true;
    }
}