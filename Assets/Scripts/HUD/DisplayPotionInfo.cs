using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayPotionInfo : MonoBehaviour
{
    public GameObject consumableSlotPrefab;
    public Transform contentPanel;
    public Sprite defaultSprite;

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


            Transform sprite = newSlot.transform.Find("sprite");
            Image image = sprite.GetComponent<Image>();
            if (image == null)
            {
                Debug.LogError("image not there on prefab");
                continue;
            }

            // Sprite potionSprite = Resources.Load<Sprite>("Potions/" + consumableType.ToString());
            Sprite potionSprite = ResourcesCache.Instance.GetSprite("Potions/" + consumableType.ToString());

            if (sprite != null)
            {
                image.sprite = potionSprite;
            }
            else
            {
                image.sprite = defaultSprite;
            }
        }
    }
}
