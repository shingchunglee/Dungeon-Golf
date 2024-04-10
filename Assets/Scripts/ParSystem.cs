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

    void Start()
    {
        updateParText();
    }

    public void oneStroke()
    {
        strokesTaken++;
        updateParText();

        if (strokesTaken > Par)
        {
            playerHealth.TakeDamage(damagePerStrokeOverPar);
        }
    }

    void updateParText()
    {
        parText.text = strokesTaken.ToString() + "/" + Par.ToString();
    }
}
