using UnityEngine;
using UnityEngine.SceneManagement;

public class EndSceneManager : MonoBehaviour
{
    public GameObject firstImage;
    public GameObject secondImage;
    public string nextSceneName = "Main Menu";
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.Instance.PlayThemeMusic();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (firstImage.activeSelf == true)
            {
                firstImage.SetActive(false);
                secondImage.SetActive(true);
                return;
            }
            // if (firstImage.activeSelf == false)
            // {
            //     LoadNextScene();
            // }
        }
    }

    public void LoadNextScene()
    {
        if (nextSceneName == "Main Menu")
        {
            var GameManagerGOPlus = GameObject.Find("GameManager +");

            if (GameManagerGOPlus != null)
            {
                SceneManager.MoveGameObjectToScene(GameManagerGOPlus, SceneManager.GetActiveScene());
            }
        }

        SceneManager.LoadScene(nextSceneName);
    }
}
