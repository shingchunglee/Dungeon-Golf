using System.Collections.Generic;
using UnityEngine;

class PlayerStatusEffect
{

    public enum StatusEffectType
    {
        STRENGTH
    }

    public class StatusEffect
    {
        public StatusEffectType type;
        public int turns;

        public virtual void OnAimEnter() { }

        public virtual void OnAimExit() { }

        public virtual void OnDamageEnemy(EnemyUnit enemy, int damage) { }

        public virtual void OnMoveEnter() { }

        public virtual void OnMoveExit() { }

        public virtual void OnChangeClub(ClubType club) { }
    }

    public List<StatusEffect> statusEffects = new();

    public StatusEffect Add(StatusEffectType effect, int turns)
    {
        var effectToAdd = statusEffects.Find(x => x.type == effect);
        if (effectToAdd == null)
        {
            // effectToAdd = new StatusEffect() { type = effect, turns = turns };
            effectToAdd = Factory(effect, turns);
            statusEffects.Add(effectToAdd);
        }
        else if (effectToAdd.turns < turns)
        {
            effectToAdd.turns = turns;
        }

        return effectToAdd;
    }

    public void Remove(StatusEffectType effect)
    {
        var effectToRemove = statusEffects.Find(x => x.type == effect);
        if (effectToRemove != null)
        {
            statusEffects.Remove(effectToRemove);
        }
        else
        {
            Debug.LogWarning($"PlayerStatusEffectList: Could not remove {effect} because it was not found in the list.");
        }
    }

    public StatusEffect Factory(StatusEffectType effect, int turns)
    {
        switch (effect)
        {
            case StatusEffectType.STRENGTH:
                return new PlayerStrength(turns);
            default:
                return null;
        }
    }
}