using System;
using TMPro;
using UnityEngine;

public class ConsumablesText : MonoBehaviour
{
    public TextMeshProUGUI Text;

    private void Awake()
    {
        Text = GameObject.Find("Consumables text") != null ? GameObject.Find("Consumables text").GetComponent<TextMeshProUGUI>() : null;
        ChestController.OnConsumableAdded += UpdateText;
    }

    public void UpdateText(Consumables consumable)
    {
        if (Text != null)
        {
            string text = "";
            foreach (Consumables consumableType in Enum.GetValues(typeof(Consumables)))
            {
                text += $"{consumableType}: {PlayerManager.Instance.inventoryController.consumables.GetConsumable(consumableType)}\n";
            }
            Text.text = text;
        }
    }
}