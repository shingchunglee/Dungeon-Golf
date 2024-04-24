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
        
        collider.sharedMaterial = null;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("ball"))
        {
            
            collider.sharedMaterial = dampenerMaterial;
            collider.enabled = false; 
            collider.enabled = true;
        }
    }

    
    
}


