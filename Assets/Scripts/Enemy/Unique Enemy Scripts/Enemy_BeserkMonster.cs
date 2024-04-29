using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_BeserkMonster : EnemyUnit
{
    //This is called after each attack.
    protected override void PostAttack()
    {
        //The enemy gains more attack after each attack done on the player.
        attackDamage += 1;
    }
}
