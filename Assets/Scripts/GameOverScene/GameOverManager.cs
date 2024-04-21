using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    // public void LoadMainMenu()
    // {
    //     SceneManager.LoadScene("Main Menu"); // Replace "MainMenu" with your main menu scene name
    // }

    private void Start()
    {
        var GameManagerGO = GameObject.Find("GameManager");
        var PlayerManagerGO = GameObject.Find("PlayerManager");

        if (GameManagerGO != null)
        {
            SceneManager.MoveGameObjectToScene(GameManagerGO, SceneManager.GetActiveScene());
        }

        if (PlayerManagerGO != null)
        {
            SceneManager.MoveGameObjectToScene(PlayerManagerGO, SceneManager.GetActiveScene());
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void RetryGame()
    {
        SceneManager.LoadScene("Main Menu"); // Reloads the current scene
    }
}
