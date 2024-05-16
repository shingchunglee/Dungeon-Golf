using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayPotionInfo : MonoBehaviour
{
    public GameObject consumableSlotPrefab; 
    public Transform contentPanel;

    void Start()
    {
        PopulatePotionList();
    }

    void PopulatePotionList()
    {
        
        InventoryConsumables consumables = PlayerManager.Instance.inventoryController.consumables;
        var consumableTypes = System.Enum.GetValues(typeof(Consumables));

        
        Debug.Log("Loaded " + consumableTypes.Length + " consumables.");

        if (consumableTypes.Length == 0)
        {
            Debug.LogError("No consumables loaded");
            return;
        }

        foreach (Consumables consumableType in consumableTypes)
        {
            int amount = consumables.GetConsumable(consumableType);
            GameObject newSlot = Instantiate(consumableSlotPrefab, contentPanel);
            if (newSlot == null)
            {
                Debug.LogError("No consumable slot prefab");
                continue;
            }

            TextMeshProUGUI text = newSlot.GetComponentInChildren<TextMeshProUGUI>();
            if (text == null)
            {
                Debug.LogError("TMP not there on prefab");
                continue;
            }

            text.text = $"{consumableType}\nAmount: {amount}";
        }
    }
}
