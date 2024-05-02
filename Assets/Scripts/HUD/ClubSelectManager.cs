using TMPro;
using UnityEngine;

public class ClubSelectManager : MonoBehaviour
{
  [SerializeField] TextMeshProUGUI clubNameText;

  private void Awake()
  {
    InventoryController.OnClubChanged += OnClubChanged;
  }

  private void OnClubChanged(Club club)
  {
    clubNameText.text = club.clubName;
  }

  public void NextClub()
  {
    PlayerManager.Instance.inventoryController.GetNextClub();
  }
}