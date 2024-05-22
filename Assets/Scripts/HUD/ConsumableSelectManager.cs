using TMPro;
using UnityEngine;

public class ConsumableSelectManager : MonoBehaviour
{
  [SerializeField] TextMeshProUGUI consumableNameText;

  private void Awake()
  {
    InventoryController.OnConsumableChanged += OnConsumableChanged;
  }

  private void OnConsumableChanged(SelectedConsumable consumable)
  {
    consumableNameText.text = consumable.Type.ToString() + ":" + consumable.Amount.ToString();
  }

  public void NextConsumable()
  {
    PlayerManager.Instance.inventoryController.GetNextConsumable();
  }

  public void UseConsumable()
  {
    PlayerManager.Instance.inventoryController.ConsumeConsumable();
  }
}