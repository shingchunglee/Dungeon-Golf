using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public float health = 100f;
    public TextMeshProUGUI healthText;
    public Transform playerTransform;
    private Vector3 lastShotPosition;
    public string gameOverSceneName = "GameOverScene";

    void Update()
    {
        healthText.text = "HP: " + health.ToString("F0");
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        UpdateHealthText();

        if (health <= 0)
        {
            GameOver();
        }
    }

    public void UpdateHealthText()
    {
        healthText.text = "Health: " + health.ToString("F0");
    }

    public void SetLastShotPosition(Vector3 position)
    {
        lastShotPosition = position;
    }

    public void Respawn()
    {
        playerTransform.position = lastShotPosition;
        health = 100f;
        UpdateHealthText();
    }

    void GameOver()
    {
        SceneManager.LoadScene(gameOverSceneName);
    }
}