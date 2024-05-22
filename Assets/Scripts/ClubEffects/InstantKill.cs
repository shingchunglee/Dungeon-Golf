using UnityEngine;

public class InstantKill : ClubEffects
{
    private const float instantKillChance = 0.3f;
    private const int highDamageAmount = 50; // Define the high damage amount

    public override void OnDamageEnemy(EnemyUnit enemy, int damage)
    {
        if (Random.value <= instantKillChance)
        {
            enemy.CurrentHP = 0; // Set the enemy's health to zero
        }
        else
        {
            enemy.TakeDamage(highDamageAmount); // Deal high damage
        }
        enemy.CheckIfDead(); // Check if the enemy should be killed
    }
}
