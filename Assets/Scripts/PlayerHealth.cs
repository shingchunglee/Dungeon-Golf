using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public float maxhealth = 100f;
    public float currentHealth;
    public TextMeshProUGUI healthText;
    public Transform playerTransform;
    private Vector3 lastShotPosition;
    public string gameOverSceneName = "GameOverScene";

    public healthBar healthBar;

    void Start()
    {
        currentHealth = maxhealth;
        healthBar.SetMaxHealth(maxhealth);

    }

    void Update()
    {
        healthText.text = "HP: " + currentHealth.ToString("F0");
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        healthBar.SetHealth(currentHealth);

        UpdateHealthText();



        if (currentHealth <= 0)
        {
            GameOver();
        }
    }

    public void UpdateHealthText()
    {
        healthText.text = "Health: " + currentHealth.ToString("F0");
    }

    public void SetLastShotPosition(Vector3 position)
    {
        lastShotPosition = position;
    }

    public void Respawn()
    {
        playerTransform.position = lastShotPosition;
        currentHealth = 100f;
        UpdateHealthText();
    }

    void GameOver()
    {
        SceneManager.LoadScene(gameOverSceneName);
    }
}