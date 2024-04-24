using UnityEngine;

public class StickySlimeEnemy : EnemyBehaviour
{
    public float slowDownMultiplier = 0.75f;



    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "ball")
        {
            PlayerManager.Instance.playerBall.GetComponent<Rigidbody2D>().velocity *= slowDownMultiplier;
        }
    }
}