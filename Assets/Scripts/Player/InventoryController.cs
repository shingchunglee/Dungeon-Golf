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
#nullable enable
  public SelectedConsumable? selectedConsumable = null;
#nullable disable
  public static event Action<Club> OnClubChanged;
  public static event Action<SelectedConsumable> OnConsumableChanged;

  private void Awake()
  {
    Init();
    consumables.AddConsumable(Consumables.EXPLOSION_POTION, 5);
    OnClubChanged += (Club club) =>
    {
      foreach (var effects in Enum.GetValues(typeof(ClubEffectsType)).Cast<ClubEffectsType>())
      {
        if (Array.Exists(club.clubEffectsTypes, effect => effect == effects))
        {
          ClubEffectsFactory.Create(effects).OnClubChanged(club);
        }
        else
        {
          ClubEffectsFactory.Create(effects).OnClubRemoved(club);
        }
      }
    };
    // AddClub(ClubType.LegendaryClub);
  }

  private void Start()
  {

  }

  public void Init()
  {
    if (clubs.Count == 0)
    {
      AddClub(ClubType.Iron7);
      AddClub(ClubType.stonePutter);
      AddClub(ClubType.ironPutter);
      AddClub(ClubType.ironWedge);
      AddClub(ClubType.ironDriver);
      AddClub(ClubType.stoneWedge);
    }
    UpdateUI();
  }

  public void UpdateUI()
  {
    OnClubChanged?.Invoke(GetSelectedClub());

    int selected = selectedConsumable?.Type == null ? 0 : (int)selectedConsumable.Type;
    selectedConsumable = new((Consumables)selected, consumables.GetConsumable((Consumables)selected));
    OnConsumableChanged?.Invoke(selectedConsumable);
  }

  private void Update()
  {

  }

  public void ConsumeConsumable()
  {
    Consumable item = ConsumableFactory.Factory(selectedConsumable.Type);
    item.Consume();
    UpdateConsumable();
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
    if (PlayerManager.Instance.actionStateController.currentState != PlayerManager.Instance.actionStateController.aimState) return;
    selectedClubIndex = (selectedClubIndex + 1) % clubs.Count();
    OnClubChanged?.Invoke(GetSelectedClub());
  }

  public void GetPreviousClub()
  {
    if (PlayerManager.Instance.actionStateController.currentState != PlayerManager.Instance.actionStateController.aimState) return;
    selectedClubIndex = (selectedClubIndex + clubs.Count() - 1) % clubs.Count();
    OnClubChanged?.Invoke(GetSelectedClub());
  }

  public void GetNextConsumable()
  {
    var consumableTypes = Enum.GetValues(typeof(Consumables));

    int selected = selectedConsumable?.Type == null ? 0 : (int)selectedConsumable.Type;

    selected = (selected + 1) % consumableTypes.Length;
    selectedConsumable = new((Consumables)selected, consumables.GetConsumable((Consumables)selected));
    OnConsumableChanged?.Invoke(selectedConsumable);
  }

  public void GetPreviousConsumable()
  {
    var consumableTypes = Enum.GetValues(typeof(Consumables));

    int selected = selectedConsumable?.Type == null ? 0 : (int)selectedConsumable.Type;

    selected = (selected + consumableTypes.Length - 1) % consumableTypes.Length;
    selectedConsumable = new((Consumables)selected, consumables.GetConsumable((Consumables)selected));
    OnConsumableChanged?.Invoke(selectedConsumable);
  }

  public void UpdateConsumable()
  {

    var consumableTypes = Enum.GetValues(typeof(Consumables));

    int selected = selectedConsumable?.Type == null ? 0 : (int)selectedConsumable.Type;

    selectedConsumable = new((Consumables)selected, consumables.GetConsumable((Consumables)selected));
    OnConsumableChanged?.Invoke(selectedConsumable);
  }

  internal void EquipClub(InventoryClub club)
  {
    if (PlayerManager.Instance.actionStateController.currentState != PlayerManager.Instance.actionStateController.aimState) return;
    var index = clubs.FindIndex(x => x.Type == club.Type);
    selectedClubIndex = index;
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

  public bool ConsumeConsumable(Consumables consumableType, int amount)
  {
    if (consumables[consumableType] <= 0) return false;
    consumables[consumableType] -= amount;
    return true;
  }

  public int GetConsumable(Consumables consumableType)
  {
    return consumables[consumableType];
  }
}

public class SelectedConsumable
{
  public Consumables Type;
  public int Amount;

  public SelectedConsumable(Consumables type, int amount)
  {
    Type = type;
    Amount = amount;
  }
}