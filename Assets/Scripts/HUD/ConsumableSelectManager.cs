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
}