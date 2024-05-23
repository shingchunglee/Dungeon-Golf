using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum EnemyStatusEffectType
{
    Frozen,
    Fire,
    Stun,
    Curse,
}

[System.Serializable]
public class EnemyStatusEffect
{
    public EnemyStatusEffectType type;
    public int turns;

    public EnemyStatusEffectPriority priority = new();

    public Action<EnemyUnit> OnTakeTurn;
}

[System.Serializable]
public class EnemyStatusEffectPriority
{
    public int onTakeTurn = 0;
}

[System.Serializable]
public class EnemyStatusEffectList
{
    public List<EnemyStatusEffect> statusEffects = new();

    private GameObject statusEffectUI;

    public EnemyStatusEffectList(GameObject statusEffectUI)
    {
        this.statusEffectUI = statusEffectUI;
    }

    public EnemyStatusEffect Add(EnemyStatusEffectType effect, int turns)
    {
        var effectToAdd = statusEffects.Find(x => x.type == effect);
        if (effectToAdd == null)
        {
            effectToAdd = new EnemyStatusEffect() { type = effect, turns = turns };
            statusEffects.Add(effectToAdd);
            AddIcon(effect, turns, statusEffectUI);
        }
        else if (effectToAdd.turns < turns)
        {
            effectToAdd.turns = turns;
        }

        return effectToAdd;
    }

    public void Remove(EnemyStatusEffectType effect)
    {
        var effectToRemove = statusEffects.Find(x => x.type == effect);
        if (effectToRemove != null)
        {
            statusEffects.Remove(effectToRemove);
            RemoveIcon(effectToRemove.type, statusEffectUI);
        }
        else
        {
            Debug.LogWarning($"EnemyStatusEffectList: Could not remove {effect} because it was not found in the list.");
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
                Remove(statusEffects[i].type);
            }
            else
            {
                UpdateTurnsText(statusEffects[i].type, statusEffects[i].turns, statusEffectUI);
            }
        }
    }

    public void AddIcon(EnemyStatusEffectType effect, int effectTurns, GameObject parent)
    {
        if (parent == null) return;
        // GameObject icon = Resources.Load<GameObject>("StatusEffects/" + effect.ToString());
        GameObject icon = ResourcesCache.Instance.GetPrefab("StatusEffects/" + effect.ToString());

        Transform oldIcon = parent.transform.Find(effect.ToString());
        if (oldIcon != null) return;

        if (icon != null)
        {
            GameObject newIcon = GameObject.Instantiate(icon, parent.transform);
            newIcon.name = effect.ToString();
            Transform turns = newIcon.transform.Find("Turns");
            turns.gameObject.GetComponent<TextMeshProUGUI>().text = effectTurns.ToString();
        }
    }

    public void UpdateTurnsText(EnemyStatusEffectType effect, int effectTurns, GameObject parent)
    {
        if (parent == null) return;
        Transform icon = parent.transform.Find(effect.ToString());
        if (icon == null) return;
        Transform turns = icon.transform.Find("Turns");
        turns.gameObject.GetComponent<TextMeshProUGUI>().text = effectTurns.ToString();
    }

    public void RemoveIcon(EnemyStatusEffectType effect, GameObject parent)
    {
        if (parent == null) return;
        Transform icon = parent.transform.Find(effect.ToString());
        if (icon == null) return;
        GameObject.Destroy(icon.gameObject);
    }
}