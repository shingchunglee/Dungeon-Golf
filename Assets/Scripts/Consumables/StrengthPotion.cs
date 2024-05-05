using System;
using UnityEngine;

// public class StrengthPotion : Consumable
// {
//     public Consumables type = Consumables.STRENGTH_POTION;
//     public static event Action OnConsume = delegate { };

//     public override void Consume()
//     {
//         Debug.Log("Consumed Strength Potion!");
//         if (PlayerManager.Instance.inventoryController.consumables.ConsumeConsumable(type, 1))
//         {
//             OnConsume.Invoke();
//         }
//     }
// }