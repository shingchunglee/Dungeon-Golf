using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
  [SerializeField]
  private List<InventoryClub> clubs = new();
  [SerializeField]
  public InventoryConsumables consumables = new();
  private int selectedClubIndex = 0;
  public static event Action<Club> OnClubChanged;

  private void Start()
  {
    AddClub(ClubType.Iron7);
    // AddClub(ClubType.LegendaryClub);
    OnClubChanged?.Invoke(GetSelectedClub());
  }

  public InventoryClub[] GetInventoryClubs()
  {
    return clubs.ToArray();
  }

  public InventoryClub AddClub(ClubType type)
  {
    InventoryClub inventoryClub = new(type, ClubFactory.Factory(type));
    clubs.Add(inventoryClub);
    OnClubChanged?.Invoke(GetSelectedClub());
    return inventoryClub;
  }

  public Club GetSelectedClub()
  {
    return clubs[selectedClubIndex].Club;
  }

  public void GetNextClub()
  {
    selectedClubIndex = (selectedClubIndex + 1) % clubs.Count();
    OnClubChanged?.Invoke(GetSelectedClub());
  }
}

[Serializable]
public struct InventoryClub
{
  public ClubType Type;
  public Club Club;

  public InventoryClub(ClubType type, Club club)
  {
    Type = type;
    Club = club;
  }
}

[Serializable]
public class InventoryConsumables
{
  private readonly Dictionary<Consumables, int> consumables = new();

  public InventoryConsumables()
  {
    foreach (Consumables consumable in Enum.GetValues(typeof(Consumables)))
    {
      consumables.Add(consumable, 0);
    }
  }

  public void AddConsumable(Consumables consumableType, int amount)
  {
    consumables[consumableType] += amount;
  }

  public void ConsumeConsumable(Consumables consumableType, int amount)
  {
    consumables[consumableType] -= amount;
  }

  public int GetConsumable(Consumables consumableType)
  {
    return consumables[consumableType];
  }
}

