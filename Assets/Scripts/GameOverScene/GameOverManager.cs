using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Main Menu"); // Replace "MainMenu" with your main menu scene name
    }

    public void RetryGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reloads the current scene
    }
}
