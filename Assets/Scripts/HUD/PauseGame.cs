using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour
{
    public GameObject InventoryMenu;
    public GameObject PauseMenu;

    void Update()
    {
        // Check if either InventoryMenu or PauseMenu is active
        if (InventoryMenu.activeSelf || PauseMenu.activeSelf)
        {
            Pause();
        }
        else
        {
            Continue();
        }
    }

    public void Pause()
    {
        if (Time.timeScale != 0) // Only pause if the game isn't already paused
        {
            Time.timeScale = 0;
            Debug.Log("Game Paused");
        }
    }

    public void Continue()
    {
        if (Time.timeScale != 1) // Only continue if the game isn't already running
        {
            Time.timeScale = 1;
            Debug.Log("Game Continued");
        }
    }
}