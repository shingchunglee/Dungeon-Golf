using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public int MaxEnemyHP;
    public int CurrentEnemyHP;

    // Start is called before the first frame update
    void Start()
    {
        CurrentEnemyHP = MaxEnemyHP;
    }

    private void OnCollisionEnter2D(Collider2D collision)
    {
       if (collision.gameObject.CompareTag("ball"))
        {
            dealDamage(1);
        }
    }

   void dealDamage(int amount)
   {
    CurrentEnemyHP -= amount;

    if(CurrentEnemyHP <= 0)
    {
        Destroy (gameObject);
    }

   }
}
