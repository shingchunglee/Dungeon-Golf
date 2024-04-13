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

    void GameOver()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}