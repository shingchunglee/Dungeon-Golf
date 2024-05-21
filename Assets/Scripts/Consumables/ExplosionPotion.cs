using System;
using UnityEngine;

public class ExplosionPotion : Consumable
{
  public Consumables type = Consumables.EXPLOSION_POTION;
  public static event Action OnConsume = delegate { };

  public override void Consume()
  {
    Debug.Log("Consumed Explosion Potion!");
    if (PlayerManager.Instance.inventoryController.consumables.ConsumeConsumable(type, 1))
    {
      OnConsume.Invoke();
    }
  }
}