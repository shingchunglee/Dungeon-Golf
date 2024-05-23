using UnityEngine;

class PlayerInstakill : PlayerStatusEffect.StatusEffect
{
    public PlayerInstakill(int turns)
    {
        type = PlayerStatusEffect.StatusEffectType.INSTAKILL;
        this.turns = turns;
        this.isInfinite = true;
    }
}