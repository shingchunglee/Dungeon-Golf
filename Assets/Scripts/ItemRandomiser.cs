using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemRandomiser : MonoBehaviour
{
    private static ItemRandomiser instance;

    public static ItemRandomiser Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ItemRandomiser>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("ItemRandomiser");
                    instance = obj.AddComponent<ItemRandomiser>();
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public ClubType? GetRandomisedClub()
    {
        ClubType[] clubTypes = Enum.GetValues(typeof(ClubType)).Cast<ClubType>().ToArray();
        ClubType[] clubsInInventory = PlayerManager.Instance.inventoryController.GetInventoryClubs().Select(x => x.Type).ToArray();

        clubTypes = clubTypes.Except(clubsInInventory).ToArray();

        List<InventoryClub> clubs = new();
        int weights = 0;

        foreach (ClubType clubType in clubTypes)
        {
            Club club = ClubFactory.Factory(clubType);
            clubs.Add(new InventoryClub(clubType, club));
            weights += club.weight;
        }

        int randomInt = SeededRandom.Range(SeededRandom.Instance.ItemRandom, 0, weights);

        foreach (InventoryClub club in clubs)
        {
            if (randomInt < club.Club.weight)
            {
                // TODO return clubType here
                return club.Type;
            }
            randomInt -= club.Club.weight;
        }

        return null;
    }

    public Dictionary<Consumables, int> GetRandomConsumables()
    {
        int random = SeededRandom.Range(SeededRandom.Instance.ItemRandom, 0, Enum.GetValues(typeof(Consumables)).Length); // get 0 to 2 potions
        var randomConsumables = new Dictionary<Consumables, int>
        {
            { (Consumables)random, SeededRandom.Range(SeededRandom.Instance.ItemRandom, 0, 3) }
        };
        return randomConsumables;
    }
}
