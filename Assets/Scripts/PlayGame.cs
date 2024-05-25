using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayGame : MonoBehaviour
{
    public string nextSceneName = "BEGIN";
    // Start is called before the first frame update
    public void PlayGameB()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
