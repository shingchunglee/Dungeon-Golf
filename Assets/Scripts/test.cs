using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    private PlayerHealth playerHealth;

    private void Start()
    {
        playerHealth = FindObjectOfType<PlayerHealth>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            playerHealth?.TakeDamage(30); 
        }
    }
}