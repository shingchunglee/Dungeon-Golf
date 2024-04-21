using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class ParSystem : MonoBehaviour
{
    public int Par = 6;
    private int strokesTaken = 0;
    public TextMeshProUGUI parText;
    public int damagePerStrokeOverPar = 5;
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
        PlayerManager.Instance.SetLastShotPosition(playerTransform.position);

        if (strokesTaken > Par)
        {
            PlayerManager.Instance.TakeDamage(damagePerStrokeOverPar);
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
            PlayerManager.Instance.TakeDamage(20);

            if (PlayerManager.Instance.currentHP <= 0)
            {
                PlayerManager.Instance.Respawn();
            }

        }
    }
}

