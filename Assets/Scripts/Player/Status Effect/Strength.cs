using UnityEngine;

class PlayerStrength : PlayerStatusEffect.StatusEffect
{
    public PlayerStrength(int turns)
    {
        type = PlayerStatusEffect.StatusEffectType.STRENGTH;
        this.turns = turns;
    }

    public override void OnDamageEnemy(EnemyUnit enemy, ref int damage)
    {
        damage = Mathf.FloorToInt(damage * 1.2f);
    }
}