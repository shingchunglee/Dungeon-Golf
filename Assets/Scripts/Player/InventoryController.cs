using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
  [SerializeField]
  private List<InventoryClub> clubs = new();
  private int selectedClubIndex = 0;
  public static event Action<Club> OnClubChanged;

  private void Start()
  {
    AddClub(ClubType.Iron7);
    // clubs.Add(ClubFactory.Factory(ClubType.Iron7));
    // clubs.Add(ClubFactory.Factory(ClubType.LegendaryClub));
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