using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerStatusEffect
{

    public enum StatusEffectType
    {
        STRENGTH
    }

    [System.Serializable]
    public class StatusEffect
    {
        public StatusEffectType type;
        public int turns;

        public virtual void OnAimEnter() { }

        public virtual void OnAimExit() { }

        public virtual void OnDamageEnemy(EnemyUnit enemy, ref int damage) { }

        public virtual void OnMoveEnter() { }

        public virtual void OnMoveExit() { }

        public virtual void OnChangeClub(ClubType club) { }
    }

    public GameObject statusEffectUI;

    public List<StatusEffect> activeStatusEffects = new();

    public PlayerStatusEffect(GameObject statusEffectUI)
    {
        this.statusEffectUI = statusEffectUI;
    }

    public StatusEffect Add(StatusEffectType effect, int turns)
    {
        var effectToAdd = activeStatusEffects.Find(x => x.type == effect);
        if (effectToAdd == null)
        {
            // effectToAdd = new StatusEffect() { type = effect, turns = turns };
            effectToAdd = Factory(effect, turns);
            activeStatusEffects.Add(effectToAdd);
            AddIcon(effect, turns, statusEffectUI);
        }
        else if (effectToAdd.turns < turns)
        {
            effectToAdd.turns = turns;
        }

        return effectToAdd;
    }

    public void TurnPassed()
    {
        for (int i = activeStatusEffects.Count - 1; i >= 0; i--)
        {
            activeStatusEffects[i].turns--;
            if (activeStatusEffects[i].turns <= 0)
            {
                Remove(activeStatusEffects[i].type);
            }
            else
            {
                UpdateTurnsText(activeStatusEffects[i].type, activeStatusEffects[i].turns, statusEffectUI);
            }
        }
    }

    public void Remove(StatusEffectType effect)
    {
        var effectToRemove = activeStatusEffects.Find(x => x.type == effect);
        if (effectToRemove != null)
        {
            activeStatusEffects.Remove(effectToRemove);
            RemoveIcon(effectToRemove.type, statusEffectUI);
        }
        else
        {
            Debug.LogWarning($"PlayerStatusEffectList: Could not remove {effect} because it was not found in the list.");
        }
    }

    public void UpdateIcons()
    {
        for (int i = 0; i < activeStatusEffects.Count; i++)
        {
            AddIcon(activeStatusEffects[i].type, activeStatusEffects[i].turns, statusEffectUI);
            UpdateTurnsText(activeStatusEffects[i].type, activeStatusEffects[i].turns, statusEffectUI);
        }
    }

    public void AddIcon(StatusEffectType effect, int effectTurns, GameObject parent)
    {
        if (parent == null) return;
        GameObject icon = Resources.Load<GameObject>("StatusEffects/Player/" + effect.ToString());

        Transform oldIcon = parent.transform.Find(effect.ToString());
        if (oldIcon != null) return;

        if (icon != null)
        {
            GameObject newIcon = GameObject.Instantiate(icon, parent.transform);
            newIcon.name = effect.ToString();
            Transform turns = newIcon.transform.Find("Turns");
            turns.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = effectTurns.ToString();
        }
    }

    public void UpdateTurnsText(StatusEffectType effect, int effectTurns, GameObject parent)
    {
        if (parent == null) return;
        Transform icon = parent.transform.Find(effect.ToString());
        if (icon == null) return;
        Transform turns = icon.transform.Find("Turns");
        turns.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = effectTurns.ToString();
    }

    public void RemoveIcon(StatusEffectType effect, GameObject parent)
    {
        if (parent == null) return;
        Transform icon = parent.transform.Find(effect.ToString());
        if (icon == null) return;
        GameObject.Destroy(icon.gameObject);
    }


    public bool Contains(StatusEffectType effect)
    {
        return activeStatusEffects.Find(x => x.type == effect) != null;
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