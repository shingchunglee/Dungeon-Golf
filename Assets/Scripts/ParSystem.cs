using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class NewBehaviourScript : MonoBehaviour
{
    public int Par = 6; 
    private int strokesTaken = 0; 
    public TextMeshProUGUI parText;
    public PlayerHealth playerHealth;
    public float damagePerStrokeOverPar = 5f;
    public Transform playerTransform;
    private Vector3 lastShotPosition;

    void Start()
    {
        updateParText();
        lastShotPosition = playerTransform.position; 
    }

    public void oneStroke()
    {
        strokesTaken++;
        lastShotPosition = playerTransform.position; 
        updateParText();
        playerHealth.SetLastShotPosition(playerTransform.position);

        if (strokesTaken > Par)
        {
            playerHealth.TakeDamage(damagePerStrokeOverPar);
        }
    }

    void updateParText()
    {
        parText.text = strokesTaken.ToString() + "/" + Par.ToString();
    }
    
    void OnTriggerExit(Collider other) // Trigger is to check if player leaves an invisible boundary
    {
        if (other.CompareTag("Player")) // Make sure the player has a tag "Player"
        {
            playerHealth.TakeDamage(20); 
            if (playerHealth.health <= 0)
            {
                playerHealth.Respawn(); 
            }
            
        }
    }
}

