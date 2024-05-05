using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{

  public bool isProceduralGen = true;
  public GameObject enemyParentObj;
  public bool isCursorOverHUDElement = false;
  public EnemyManager enemyManager;
  private static GameManager _instance;
  public static GameManager Instance
  {
    get
    {
      if (_instance == null)
      {
        _instance = FindObjectOfType<GameManager>();

        if (_instance == null)
        {
          Debug.LogError("No GameManager instance found in the scene, " +
                         "ensure one exists in the first loaded scene");
        }
      }

      return _instance;
    }
  }
  [HideInInspector]
  public bool isInitialized = false;

  public GridManager gridManager;

  private string gameOverSceneName = "GameOver";

  private GameObject ProcGenLevelsParent;
  public List<ProcedualGeneration> proceduralGenerationPresets;
  [HideInInspector]
  public int procGenLevelIndex = 0;
  [HideInInspector]
  public int dungeonLevel = 0;

  public ItemRandomiser itemRandomiser;
  // public GolfAimType golfAimType = GolfAimType.Click;
  public GameObject settingsCanvas;
  public GameObject inventoryCanvas;

  public TextMeshProUGUI HPText;
  public bool isSettingsOpen = false;
  public bool isInventoryOpen = false;

  public StatsController statsController;
  public string nextSceneName = "";

  private void Awake()
  {
    if (_instance != null && _instance != this)
    {
      Debug.LogError("More than one GameManager instance detected in the scene, " +
                     "ensure only one exists in the first loaded scene");
      Destroy(this.gameObject);
    }
    else
    {
      _instance = this;
      DontDestroyOnLoad(gameObject);
      // Init();
    }

  }

  public void InitializeThisFromOtherScript()
  {
    if (!isInitialized)
    {
      Init();
    }
  }

  private void Init()
  {
    isInitialized = true;

    enemyManager = gameObject.AddComponent<EnemyManager>();

    HPText = GameObject.Find("HP Text")?.GetComponent<TextMeshProUGUI>();

    gridManager = GameObject.Find("GridManager").GetComponent<GridManager>();

    proceduralGenerationPresets = new List<ProcedualGeneration>();
    nextSceneName = "";

    PlayerManager.Instance.Init();

    ProcGenLevelsParent = GameObject.Find("Procedural Generation Levels");
    foreach (Transform child in ProcGenLevelsParent.transform)
    {
      var procGen = child.gameObject.GetComponent<ProcedualGeneration>();

      if (procGen != null)
      {
        proceduralGenerationPresets.Add(procGen);
      }
    }
    if (isProceduralGen)
    {
      proceduralGenerationPresets[procGenLevelIndex].Main();

      proceduralGenerationPresets[procGenLevelIndex].UpdateGridManager();

      PlayerManager.Instance.PlayerSpawnInit();

      HoleGoal holeGoal = GameObject.Find("HoleGoal").GetComponent<HoleGoal>();
      holeGoal.GoalSpawnInit();
    }
  }

  private void Update()
  {
    if (Input.GetKeyDown(KeyCode.Escape) && !isSettingsOpen)
    {
      settingsCanvas.SetActive(true);
      isSettingsOpen = true;
      isCursorOverHUDElement = true;
    }
  }

  public void CloseSettings()
  {
    // SceneManager.UnloadSceneAsync("Settings");
    settingsCanvas.SetActive(false);
    isSettingsOpen = false;
    isCursorOverHUDElement = false;
  }


  /// <summary>
  /// Called from PlayerManager when player health <= 0
  /// </summary>
  public void GameOver()
  {
    Debug.Log("Game over called!");
    SceneManager.LoadScene(gameOverSceneName);
  }

  public void AdvanceLevel()
  {
    dungeonLevel++;

    isInitialized = false;

    if (procGenLevelIndex < proceduralGenerationPresets.Count)
    {
      procGenLevelIndex++;
    }

    // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    nextSceneName = "";
    SceneManager.LoadScene("LoadingScene");
  }

  public void AdvanceLevelSpecific(string levelName)
  {
    dungeonLevel++;

    isInitialized = false;
    nextSceneName = levelName;
    SceneManager.LoadScene("LoadingScene");
  }

  private void LoadStats()
  {
    GameObject statsText = GameObject.Find("StatsText");
    TextMeshProUGUI text = statsText.GetComponentInChildren<TextMeshProUGUI>();

    text.text = statsController.GetStatsToString();
  }

  public void LoadGameScene()
  {
    SceneManager.LoadScene("GameScene");
  }

  public void LoadNextLevel()
  {
    if (nextSceneName == "")
    {
      LoadGameScene();
    }
    else
    {
      SceneManager.LoadScene(nextSceneName);
    }
  }

  private void OnEnable()
  {
    SceneManager.sceneLoaded += OnSceneLoaded;
  }

  private void OnDisable()
  {
    SceneManager.sceneLoaded -= OnSceneLoaded;
  }

  private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
  {
    if (scene.name == "LoadingScene")
    {
      LoadStats();
    }
    else
    {
      //This trys to run Init if GameManager and PlayerManager Exist.
      try
      {
        // PlayerManager.Instance.Init();   //This is called in GameManager.Init()
        this.Init();
      }
      catch (Exception ex)
      {
        Debug.LogWarning("Init unsuccessful. Error: " + ex.Message);
      }
    }
  }
}