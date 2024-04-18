using System;
using UnityEngine;

public class ChestController : MonoBehaviour
{
    public static event Action<Club> OnClubAdded;

    private void Start()
    {
    }
    public void OpenChest()
    {
        Debug.Log("open chest");
        ClubType clubType = GameManager.Instance.itemRandomiser.GetRandomisedClub();

        InventoryClub club = PlayerManager.Instance.inventoryController.AddClub(clubType);
        OnClubAdded?.Invoke(club.Club);
    }
}