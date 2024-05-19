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
    }

    public List<StatusEffect> statusEffects = new();

    public StatusEffect Add(StatusEffectType effect, int turns)
    {
        var effectToAdd = statusEffects.Find(x => x.type == effect);
        if (effectToAdd == null)
        {
            effectToAdd = new StatusEffect() { type = effect, turns = turns };
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
}