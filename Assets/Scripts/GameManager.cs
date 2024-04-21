using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEditor.Animations;

public class GameManager : MonoBehaviour
{

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

  private GameObject ProcGenLevelsParent;
  public List<ProcedualGeneration> proceduralGenerationPresets;
  public int procGenLevelIndex = 0;

  public ItemRandomiser itemRandomiser;
  public GolfAimType golfAimType = GolfAimType.Click;

  public TextMeshProUGUI HPText;

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
      Init();
    }

  }

  private void Init()
  {
    enemyManager = gameObject.AddComponent<EnemyManager>();

    HPText = GameObject.Find("HP Text").GetComponent<TextMeshProUGUI>();
    

    proceduralGenerationPresets = new List<ProcedualGeneration>();

    ProcGenLevelsParent = GameObject.Find("Procedural Generation Levels");
    foreach (Transform child in ProcGenLevelsParent.transform)
    {
      var procGen = child.gameObject.GetComponent<ProcedualGeneration>();

      if (procGen != null)
      {
        proceduralGenerationPresets.Add(procGen);
      }
    }
    proceduralGenerationPresets[procGenLevelIndex].Main();

    PlayerManager.Instance.PlayerSpawnInit();

    HoleGoal holeGoal = GameObject.Find("HoleGoal").GetComponent<HoleGoal>();
    holeGoal.GoalSpawnInit();

  }

  /// <summary>
  /// Called from PlayerManager when player health <= 0
  /// </summary>
  public void GameOver()
  {
    Debug.Log("Game over called!");
  }

  public void AdvanceLevel()
  {
    if (procGenLevelIndex < proceduralGenerationPresets.Count)
    {
      procGenLevelIndex++;
    }

    SceneManager.LoadScene(SceneManager.GetActiveScene().name);

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
    PlayerManager.Instance.Init();
    this.Init();
  }
}

