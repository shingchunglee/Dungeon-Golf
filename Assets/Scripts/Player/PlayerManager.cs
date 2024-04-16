using UnityEngine;

public class PlayerManager : MonoBehaviour
{
  private static PlayerManager _instance;
  public PowerLevelController powerLevelController;
  public InventoryController inventoryController;

  public PlayerActionStateController actionStateController;
  // public PlayerActionStateController playerActionStateController;
  public GolfAim golfAim;
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
    transform.position = new Vector3(playerSpawn.x, playerSpawn.y, transform.position.z);
  }

  private void Init()
  {
  }

  private void OnTriggerEnter2D(Collider2D other)
  {
    actionStateController.OnTriggerEnter2D(other);
  }
}
