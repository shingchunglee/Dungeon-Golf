using UnityEngine;

class PlayerStun : PlayerStatusEffect.StatusEffect
{
    public PlayerStun(int turns)
    {
        type = PlayerStatusEffect.StatusEffectType.STUN;
        this.turns = turns;
        this.isInfinite = true;
    }
}