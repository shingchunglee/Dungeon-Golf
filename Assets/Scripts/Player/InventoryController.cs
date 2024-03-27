using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
  [SerializeField]
  private List<Club> clubs = new List<Club>();
  private int selectedClubIndex = 0;
  public static event Action<Club> OnClubChanged;

  private void Start()
  {
    clubs.Add(ClubFactory.Instance.Factory(ClubType.Iron7));
    clubs.Add(ClubFactory.Instance.Factory(ClubType.LegendaryClub));
    OnClubChanged?.Invoke(GetSelectedClub());
  }

  public Club GetSelectedClub()
  {
    return clubs[selectedClubIndex];
  }

  public void GetNextClub()
  {
    selectedClubIndex = (selectedClubIndex + 1) % clubs.Count();
    OnClubChanged?.Invoke(GetSelectedClub());
  }
}