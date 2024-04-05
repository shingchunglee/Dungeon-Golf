using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public int Lives;
    public TextMeshProUGUI lifeText;

    void Update()
    {
         lifeText.text = Lives.ToString();
    }

    
    public void AddLife()
    {
    Lives++;
    UpdateLifeText();

    
    }

    public void LoseLife()
    {
        Lives--;
        UpdateLifeText();
    }

    public void UpdateLifeText() 
    {
        lifeText.text = Lives.ToString();
    }

    
}
