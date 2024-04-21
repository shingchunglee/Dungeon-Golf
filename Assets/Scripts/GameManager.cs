using UnityEngine;
using TMPro;

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

  public ProcedualGeneration procedualGeneration;
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
      Init();
    }

    procedualGeneration.Main();
  }

  private void Init()
  {
    enemyManager = gameObject.AddComponent<EnemyManager>();
  }

  /// <summary>
  /// Called from PlayerManager when player health <= 0
  /// </summary>
  public void GameOver()
  {
    Debug.Log("Game over called!");
  }
}

