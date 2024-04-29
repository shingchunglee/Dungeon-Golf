using System;
using System.Collections.Generic;
using UnityEngine;

public class ChestController : MonoBehaviour
{
    public static event Action<Club> OnClubAdded;
    public static event Action<Consumables> OnConsumableAdded;

    private void Start()
    {
    }

    public void OpenChest()
    {
        Debug.Log("open chest");
        ClubType? clubType = ItemRandomiser.Instance.GetRandomisedClub();

        Dictionary<Consumables, int> randomConsumables = ItemRandomiser.Instance.GetRandomConsumables();

        foreach (KeyValuePair<Consumables, int> consumable in randomConsumables)
        {
            PlayerManager.Instance.inventoryController.consumables.AddConsumable(consumable.Key, consumable.Value);
            OnConsumableAdded?.Invoke(consumable.Key);
        }

        if (clubType == null) return;
        InventoryClub club = PlayerManager.Instance.inventoryController.AddClub((ClubType)clubType);
        OnClubAdded?.Invoke(club.Club);
    }
}