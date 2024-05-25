using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneManager : MonoBehaviour
{
    public GameObject firstImage;
    public GameObject secondImage;
    public string nextSceneName = "Main Menu";
    // Start is called before the first frame update
    void Start()
    {

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
            if (firstImage.activeSelf == false)
            {
                LoadNextScene();
            }
        }
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
