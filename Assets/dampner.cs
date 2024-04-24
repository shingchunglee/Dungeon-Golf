using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dampner : EnemyBehaviour
{

    public PhysicsMaterial2D dampenerMaterial;
    private Collider2D collider;

    void Start()
    {
        collider = GetComponent<Collider2D>();
        // Initially, let's assume the collider doesn't have the dampening material.
        collider.sharedMaterial = null;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("ball"))
        {
            // When the 'ball' enters the trigger, assign the dampening material to the collider.
            collider.sharedMaterial = dampenerMaterial;
            collider.enabled = false; // Refresh the collider
            collider.enabled = true;
        }
    }

    // Optionally, if you want to remove the effect when the 'ball' exits:
    
}


