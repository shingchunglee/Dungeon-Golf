using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class SceneList : MonoBehaviour
{
    private string standardScenesFolderPath = "Scenes/CustomLevels_Final/StandardLevels";
    private string challengeScenesFolderPath = "Scenes/CustomLevels_Final/ChallengeLevels";

    public List<string> StandardLevelNames = new List<string>();
    public List<string> ChallengeLevelNames = new List<string>();

    void Start()
    {
        GetSceneNames(standardScenesFolderPath, StandardLevelNames);
        GetSceneNames(challengeScenesFolderPath, ChallengeLevelNames);

    }

    void GetSceneNames(string filepatch, List<string> levelList)
    {
        string scenesPath = Path.Combine(Application.dataPath, filepatch);

        // Get all scene files in the specified folder
        string[] sceneFiles = Directory.GetFiles(scenesPath, "*.unity");

        foreach (string sceneFile in sceneFiles)
        {
            // Extract scene name from file path and add it to the list
            string sceneName = Path.GetFileNameWithoutExtension(sceneFile);
            levelList.Add(sceneName);
        }
    }
}
