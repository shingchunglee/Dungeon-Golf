using UnityEngine;

class PlayerCurse : PlayerStatusEffect.StatusEffect
{
    public PlayerCurse(int turns)
    {
        type = PlayerStatusEffect.StatusEffectType.CURSE;
        this.turns = turns;
        this.isInfinite = true;
    }
}