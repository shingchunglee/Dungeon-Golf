using System;
using UnityEngine;

public class HealthPotion : Consumable
{
    public Consumables type = Consumables.HEALTH_POTION;
    public static event Action OnConsume = delegate { };

    public override void Consume()
    {
        if (PlayerManager.Instance.IsHealthAtMax()) return;

        Debug.Log("Consumed Health Potion!");
        if (PlayerManager.Instance.inventoryController.consumables.ConsumeConsumable(type, 1))
        {
            OnConsume.Invoke();
        }
    }
}