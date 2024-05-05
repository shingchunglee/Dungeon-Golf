using System;
using System.Collections.Generic;
using UnityEngine;

public class ChestController : MonoBehaviour
{
    public static event Action<Club> OnClubAdded;
    public static event Action<Consumables> OnConsumableAdded;
    // private Rigidbody2D rb2D;

    public Vector2Int PositionOnWorldGrid
    {
        get
        {
            var position = new Vector2Int(
                Mathf.FloorToInt(transform.position.x),
                Mathf.FloorToInt(transform.position.y)
            );
            return position;
        }
    }

    public Node nodeAtLocation
    {
        get
        {
            return GameManager.Instance.gridManager.GetNodeByWorldPosition(PositionOnWorldGrid);
        }
    }


    private void Start()
    {
        // rb2D = GetComponent<Rigidbody2D>();
    }

    public void OpenChest()
    {
        Debug.Log("open chest");
        GameManager.Instance.statsController.IncrementChestsOpened();
        ClubType? clubType = ItemRandomiser.Instance.GetRandomisedClub();
        nodeAtLocation.entitiesOnTile.Remove(EntityType.CHEST);

        Dictionary<Consumables, int> randomConsumables = ItemRandomiser.Instance.GetRandomConsumables();

        foreach (KeyValuePair<Consumables, int> consumable in randomConsumables)
        {
            PlayerManager.Instance.inventoryController.consumables.AddConsumable(consumable.Key, consumable.Value);
            OnConsumableAdded?.Invoke(consumable.Key); Debug.Log(consumable.Key); Debug.Log(consumable.Value);
            PlayerManager.Instance.inventoryController.UpdateConsumable();
        }

        if (clubType == null) return;
        InventoryClub club = PlayerManager.Instance.inventoryController.AddClub((ClubType)clubType);
        OnClubAdded?.Invoke(club.Club);
        Destroy(gameObject);
    }
}