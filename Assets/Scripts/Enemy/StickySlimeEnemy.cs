using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickySlimeEnemy : MonoBehaviour
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