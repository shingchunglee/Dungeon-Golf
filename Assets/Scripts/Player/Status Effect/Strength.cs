class PlayerStrength : PlayerStatusEffect.StatusEffect
{
    public new PlayerStatusEffect.StatusEffectType type = PlayerStatusEffect.StatusEffectType.STRENGTH;

    public new int turns;

    public PlayerStrength(int turns)
    {
        type = PlayerStatusEffect.StatusEffectType.STRENGTH;
        this.turns = turns;
    }


}