using UnityEngine;

public class PlayerManager : MonoBehaviour
{
  public GameObject playerWizard;
  public GameObject playerBall;
  private static PlayerManager _instance;
  public int maxHP;
  public int currentHP;
  public int attackDamage = 5;
  public PowerLevelController powerLevelController;
  public VarianceLevelController varianceLevelController;
  public InventoryController inventoryController;
  public Transform playerTransform;
  private Vector3 lastShotPosition;

  public int voidDamage = 15;

  public PlayerActionStateController actionStateController;
  public GolfAim golfAim;
  public GolfAimDrag golfAimDrag;
  public static PlayerManager Instance
  {
    get
    {
      if (_instance == null)
      {
        _instance = FindObjectOfType<PlayerManager>();
        if (_instance == null)
        {
          GameObject container = new GameObject("PlayerManager");
          _instance = container.AddComponent<PlayerManager>();
        }
      }
      return _instance;
    }
  }

  private void Awake()
  {
    if (_instance != null && _instance != this)
    {
      Destroy(this.gameObject);
    }
    else
    {
      _instance = this;
    }

    GameStartProcessing();
    Init();
  }

  private void Start()
  {
    HealthPotion.OnConsume += RestoreHealth;
  }

  private void GameStartProcessing()
  {
    DontDestroyOnLoad(gameObject);
    currentHP = maxHP;
  }

  public void PlayerSpawnInit()
  {
    Vector2 playerSpawn = GameManager.Instance
        .proceduralGenerationPresets[GameManager.Instance.procGenLevelIndex].PlayerSpawn;

    var ballRB = playerBall.GetComponent<Rigidbody2D>();
    ballRB.position = new Vector3(playerSpawn.x, playerSpawn.y, playerBall.transform.position.z);
    TeleportPlayerToBall();
  }

  public void Init()
  {

    UpdateHPText();

    if (GameManager.Instance.HPText != null) GameManager.Instance.HPText.text = $"HP: {currentHP}/{maxHP}";

    playerWizard = GameObject.Find("Wizard Parent");
    playerBall = GameObject.Find("Ball Parent");
    var player = GameObject.Find("Player");
    powerLevelController = player.GetComponentInChildren<PowerLevelController>();
    varianceLevelController = player.GetComponentInChildren<VarianceLevelController>();
    inventoryController = GetComponentInChildren<InventoryController>();
    actionStateController = player.GetComponentInChildren<PlayerActionStateController>();

    golfAim = playerBall.GetComponentInChildren<GolfAim>();
    golfAimDrag = playerBall.GetComponentInChildren<GolfAimDrag>();
    powerLevelController = playerBall.GetComponentInChildren<PowerLevelController>();
    inventoryController.UpdateUI();
  }

  public void TakeDamage(int damage)
  {
    currentHP -= damage;
     SoundManager.Instance.PlaySFX(SoundManager.Instance.playerDamage);

    UpdateHPText();
    if (currentHP <= 0)
    {
      PlayerDies();
    }
  }

  public void RestoreHealth()
  {
    currentHP += 5;
    UpdateHPText();
  }

  public void UpdateHPText()
  {
    if (GameManager.Instance.HPText != null) GameManager.Instance.HPText.text = $"HP: {currentHP}/{maxHP}";
  }

  public void TeleportPlayerToBall()
  {
    //TODO this shouldn't be here but it works.
    UpdateHPText();

    var playerRB = playerWizard.GetComponent<Rigidbody2D>();
    var ballRB = playerBall.GetComponent<Rigidbody2D>();

    // Null safety checks
    if (playerRB == null)
    {
      Debug.LogWarning("Player RB not found!");
      return;
    }
    if (ballRB == null)
    {
      Debug.LogWarning("Ball RB not found!");
      return;
    }

    var x = Mathf.Floor(ballRB.position.x);
    var y = Mathf.Floor(ballRB.position.y);

    // playerRB.MovePosition(new Vector2(x, y));
    playerRB.position = new Vector2(x, y);

  }

  public void SetLastShotPosition(Vector3 position)
  {
    lastShotPosition = position;
  }

  public void Respawn()
  {
    playerTransform.position = lastShotPosition;
    currentHP = maxHP;
    UpdateHPText();
  }

  private void PlayerDies()
  {
    GameManager.Instance.GameOver();
  }

}
