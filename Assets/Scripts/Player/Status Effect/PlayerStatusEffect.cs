using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerStatusEffect
{
    public enum StatusEffectType
    {
        STRENGTH,
        SPEEDBOOST,
        VAMPIRISM,
        INSTAKILL,
        FREEZING,
        FLAME,
        STUN,
        CURSE,
        HEALING
    }

    [System.Serializable]
    public class StatusEffect
    {
        public StatusEffectType type;
        public int turns;
        public bool isInfinite = false;

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
            AddIcon(effect, turns, statusEffectUI, effectToAdd.isInfinite);
        }
        else if (effectToAdd.turns < turns)
        {
            if (effectToAdd.isInfinite) effectToAdd.isInfinite = true;
            effectToAdd.turns = turns;
        }

        return effectToAdd;
    }

    public void TurnPassed()
    {
        for (int i = activeStatusEffects.Count - 1; i >= 0; i--)
        {
            if (activeStatusEffects[i].isInfinite) continue;
            activeStatusEffects[i].turns--;
            if (activeStatusEffects[i].turns <= 0)
            {
                Remove(activeStatusEffects[i].type);
            }
            else
            {
                UpdateTurnsText(activeStatusEffects[i].type, activeStatusEffects[i].turns, statusEffectUI, activeStatusEffects[i].isInfinite);
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
            AddIcon(activeStatusEffects[i].type, activeStatusEffects[i].turns, statusEffectUI, isInfinite: activeStatusEffects[i].isInfinite);
            UpdateTurnsText(activeStatusEffects[i].type, activeStatusEffects[i].turns, statusEffectUI, activeStatusEffects[i].isInfinite);
        }
    }

    public void AddIcon(StatusEffectType effect, int effectTurns, GameObject parent, bool isInfinite = false)
    {
        if (parent == null) return;
        // GameObject icon = Resources.Load<GameObject>("StatusEffects/Player/" + effect.ToString());
        GameObject icon = ResourcesCache.Instance.GetPrefab("StatusEffects/Player/" + effect.ToString());

        Transform oldIcon = parent.transform.Find(effect.ToString());
        if (oldIcon != null) return;

        if (icon != null)
        {
            GameObject newIcon = GameObject.Instantiate(icon, parent.transform);
            newIcon.name = effect.ToString();
            UpdateTurnsText(effect, effectTurns, parent, isInfinite);
            // Transform turns = newIcon.transform.Find("Turns");
            // turns.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = effectTurns.ToString();
        }
    }

    public void UpdateTurnsText(StatusEffectType effect, int effectTurns, GameObject parent, bool isInfinite = false)
    {
        if (parent == null) return;
        Transform icon = parent.transform.Find(effect.ToString());
        if (icon == null) return;
        Transform turns = icon.transform.Find("Turns");
        if (isInfinite)
        {
            turns.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "∞";
        }
        else
        {
            turns.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = effectTurns.ToString();
        }
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
            case StatusEffectType.VAMPIRISM:
                return new PlayerVampirism(turns);
            case StatusEffectType.INSTAKILL:
                return new PlayerInstakill(turns);
            case StatusEffectType.FREEZING:
                return new PlayerFreezing(turns);
            case StatusEffectType.FLAME:
                return new PlayerFlame(turns);
            case StatusEffectType.CURSE:
                return new PlayerCurse(turns);
            case StatusEffectType.STUN:
                return new PlayerStun(turns);
            case StatusEffectType.HEALING:
                return new PlayerHealing(turns);
            default:

                return null;
        }
    }
}