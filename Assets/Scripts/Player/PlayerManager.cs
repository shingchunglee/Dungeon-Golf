using System;
using Unity.VisualScripting;
using UnityEditor.Callbacks;
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

  public PlayerActionStateController actionStateController;
  // public PlayerActionStateController playerActionStateController;
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
    Init();
  }

  private void Start()
  {
    Vector2 playerSpawn = GameManager.Instance.procedualGeneration.PlayerSpawn;
    var ballRB = playerBall.GetComponent<Rigidbody2D>();
    ballRB.position = new Vector3(playerSpawn.x, playerSpawn.y, playerBall.transform.position.z);
    TeleportPlayerToBall();
  }

  private void Init()
  {
    currentHP = maxHP;

    if (GameManager.Instance.HPText != null) GameManager.Instance.HPText.text = $"HP: {currentHP}/{maxHP}";

    playerWizard = GameObject.Find("Wizard Parent");
    playerBall = GameObject.Find("Ball Parent");
    var player = GameObject.Find("Player");
    powerLevelController = player.GetComponentInChildren<PowerLevelController>();
    varianceLevelController = player.GetComponentInChildren<VarianceLevelController>();
    inventoryController = player.GetComponentInChildren<InventoryController>();
    actionStateController = player.GetComponentInChildren<PlayerActionStateController>();

    golfAim = playerBall.GetComponentInChildren<GolfAim>();
    powerLevelController = playerBall.GetComponentInChildren<PowerLevelController>();
  }

  public void TakeDamage(int damage)
  {
    currentHP -= damage;

    if (GameManager.Instance.HPText != null) GameManager.Instance.HPText.text = $"HP: {currentHP}/{maxHP}";
    if (currentHP <= 0)
    {
      PlayerDies();
    }
  }

  public void TeleportPlayerToBall()
  {
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

  private void PlayerDies()
  {
    GameManager.Instance.GameOver();
  }

  private void OnTriggerEnter2D(Collider2D other)
  {
    actionStateController.OnTriggerEnter2D(other);
  }
}
