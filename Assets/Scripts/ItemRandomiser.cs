using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemRandomiser : MonoBehaviour
{
    public ClubType GetRandomisedClub()
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

        return ClubType.Iron7;
    }
}