using System.Collections.Generic;

public enum EnemyStatusEffectType
{
    Frozen,
}

[System.Serializable]
public class EnemyStatusEffect
{
    public EnemyStatusEffectType type;
    public int turns;
}

[System.Serializable]
public class EnemyStatusEffectList
{
    public List<EnemyStatusEffect> statusEffects = new();

    public void Add(EnemyStatusEffect effect)
    {
        if (!statusEffects.Contains(effect))
        {
            statusEffects.Add(effect);
        }
        else if (statusEffects[statusEffects.IndexOf(effect)].turns < effect.turns)
        {
            statusEffects[statusEffects.IndexOf(effect)].turns = effect.turns;
        }
    }

    public void Remove(EnemyStatusEffect effect)
    {
        if (statusEffects.Contains(effect))
        {
            statusEffects.Remove(effect);
        }
    }

    public bool Contains(EnemyStatusEffectType effect)
    {
        return statusEffects.Find(x => x.type == effect) != null;
    }

    public void TurnPassed()
    {
        for (int i = statusEffects.Count - 1; i >= 0; i--)
        {
            statusEffects[i].turns--;
            if (statusEffects[i].turns <= 0)
            {
                statusEffects.RemoveAt(i);
            }
        }
    }
}