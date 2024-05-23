using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayClubInfo : MonoBehaviour
{
    public GameObject clubSlotPrefab;
    public Transform contentPanel;

    public Sprite defaultSprite;

    void OnEnable()
    {
        ClearClubList();
        PopulateClubList();
    }

    private void ClearClubList()
    {
        foreach (Transform child in contentPanel)
        {
            Destroy(child.gameObject);
        }
    }

    void PopulateClubList()
    {
        // Club[] clubs = Resources.LoadAll<Club>("Clubs");
        InventoryClub[] clubs = PlayerManager.Instance.inventoryController.GetInventoryClubs();
        Debug.Log("Loaded " + clubs.Length + " clubs.");

        if (clubs.Length == 0)
        {
            Debug.LogError("No clubs loaded");
            return;
        }

        foreach (InventoryClub club in clubs)
        {
            GameObject newSlot = Instantiate(clubSlotPrefab, contentPanel);
            if (newSlot == null)
            {
                Debug.LogError("noclub slot prefab");
                continue;
            }

            TextMeshProUGUI text = newSlot.GetComponentInChildren<TextMeshProUGUI>();
            if (text == null)
            {
                Debug.LogError("TMP not there on prefab");
                continue;
            }

            text.text = $"{club.Club.clubName}\nDamage: {club.Club.damage}\nMax Power: {club.Club.maxPower}";

            Transform sprite = newSlot.transform.Find("sprite");
            Image image = sprite.GetComponent<Image>();
            if (image == null)
            {
                Debug.LogError("image not there on prefab");
                continue;
            }

            if (club.Club.sprite != null)
            {
                image.sprite = club.Club.sprite;
            }
            else
            {
                image.sprite = defaultSprite;
            }

            Transform equipButtonTransform = newSlot.transform.Find("EquipButton");
            Button equipButton = equipButtonTransform.GetComponent<Button>();
            if (equipButton == null)
            {
                Debug.LogError("equip button not there on prefab");
                continue;
            }

            equipButton.onClick.AddListener(() => PlayerManager.Instance.inventoryController.EquipClub(club));

            // Favourite Toggle
            Transform favouriteToggleTransform = newSlot.transform.Find("Favourite");
            Toggle favouriteToggle = favouriteToggleTransform.GetComponent<Toggle>();
            if (favouriteToggle == null)
            {
                Debug.LogError("favourite toggle not there on prefab");
                continue;
            }

            favouriteToggle.isOn = club.Favourite;

            favouriteToggle.onValueChanged.AddListener((bool value) =>
            {
                PlayerManager.Instance.inventoryController.UpdateFavourite(club, value);
                ClearClubList();
                PopulateClubList();
            });

            Transform statusEffects = newSlot.transform.Find("statusEffects");
            if (statusEffects == null)
            {
                Debug.LogError("status effects not there on prefab");
                continue;
            }

            foreach (ClubEffectsType effectType in club.Club.clubEffectsTypes)
            {
                ClubEffects effect = ClubEffectsFactory.Create(effectType);
                AddIcon(effect.statusEffectType, statusEffects.gameObject);
            }
        }
    }

    public void AddIcon(PlayerStatusEffect.StatusEffectType effect, GameObject parent)
    {
        if (parent == null) return;
        // GameObject icon = Resources.Load<GameObject>("StatusEffects/" + effect.ToString());
        GameObject icon = ResourcesCache.Instance.GetPrefab("StatusEffects/Player/" + effect.ToString());

        Transform oldIcon = parent.transform.Find(effect.ToString());
        if (oldIcon != null) return;

        if (icon != null)
        {
            GameObject newIcon = GameObject.Instantiate(icon, parent.transform);
            newIcon.name = effect.ToString();
            Transform turns = newIcon.transform.Find("Turns");
            turns.gameObject.GetComponent<TextMeshProUGUI>().text = "";
        }
    }
}