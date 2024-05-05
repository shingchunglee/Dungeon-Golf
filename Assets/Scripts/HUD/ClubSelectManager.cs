using TMPro;
using UnityEngine;

public class ClubSelectManager : MonoBehaviour
{
  [SerializeField] TextMeshProUGUI clubNameText;
  [SerializeField] private GameObject inventoryCanvas;

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

  public void OpenInventory()
  {
    inventoryCanvas.SetActive(true);
    GameManager.Instance.isCursorOverHUDElement = true;
    GameManager.Instance.isInventoryOpen = true;
  }

  public void CloseInventory()
  {
    inventoryCanvas.SetActive(false);
    GameManager.Instance.isCursorOverHUDElement = false;
    GameManager.Instance.isInventoryOpen = false;
  }

}