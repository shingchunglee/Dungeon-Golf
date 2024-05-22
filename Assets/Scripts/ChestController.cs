using System;
using System.Collections.Generic;
using UnityEngine;

public class ChestController : MonoBehaviour
{
    [Serializable]
    public struct ConsumablesInChest
    {
        public Consumables consumables;
        public int amount;
    }

    public static event Action<Club> OnClubAdded;
    public static event Action<Consumables> OnConsumableAdded;


    [SerializeField] private bool manualContent = false;
    [SerializeField] private ClubType[] clubs = new ClubType[0];
    [SerializeField] private ConsumablesInChest[] consumables = new ConsumablesInChest[0];

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
        if (manualContent)
        {
            foreach (ClubType clubInChest in clubs)
            {
                InventoryClub club = PlayerManager.Instance.inventoryController.AddClub(clubInChest);
                OnClubAdded?.Invoke(club.Club);
            }
            foreach (ConsumablesInChest consumable in consumables)
            {
                PlayerManager.Instance.inventoryController.consumables.AddConsumable(consumable.consumables, consumable.amount);
                OnConsumableAdded?.Invoke(consumable.consumables);
                PlayerManager.Instance.inventoryController.UpdateConsumable();
            }
        }
        else
        {
            GameManager.Instance.statsController.IncrementChestsOpened();
            ClubType? clubType = ItemRandomiser.Instance.GetRandomisedClub();

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
        }

        nodeAtLocation.entitiesOnTile.Remove(EntityType.CHEST);
        Destroy(gameObject);
    }
}