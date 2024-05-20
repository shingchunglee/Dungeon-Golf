using UnityEngine;
using UnityEngine.AI;

public class PlayerManager : MonoBehaviour
{
    public GameObject PlayerParent;
    public GameObject playerWizard;
    public Rigidbody2D wizardRB;
    public GameObject playerBall;
    public Rigidbody2D ballRB;

    private static PlayerManager _instance;
    public int maxHP;
    public int currentHP;
    public float endLevelRegenPercent = 0.4f;
    public int attackDamage = 5;
    public PowerLevelController powerLevelController;
    public VarianceLevelController varianceLevelController;
    public InventoryController inventoryController;
    public Transform playerTransform;

    public Vector2Int WizardWorldPositionOnGrid
    {
        get
        {
            var position = new Vector2Int(
                Mathf.FloorToInt(wizardRB.position.x),
                Mathf.FloorToInt(wizardRB.position.y)
            );
            return position;
        }
    }
    private Vector3 lastShotPosition;

    public int voidDamage = 15;

    public PlayerActionStateController actionStateController;
    public GolfAim golfAim;
    public GolfAimDrag golfAimDrag;

    [SerializeField] HealthBar healthBar;

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

    public bool manualControlTestMode = false;

    public bool IsBallMoving
    {
        get
        {
            if (ballRB.velocity.magnitude > 0.5f)
                return true;
            else return false;
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

        // healthBar = GetComponentInChildren<HealthBar>();
        //      if (healthBar == null)
        // {
        //     Debug.LogError("HealthBar component not found on " + gameObject.name);
        // }
    }

    private void Start()
    {
        HealthPotion.OnConsume += () => { RestoreHealth(5); };
    }

    private void FixedUpdate()
    {
        if (manualControlTestMode)
        {
            BallManualControl();
        }
    }

    /// <summary>
    /// Turn this on it PlayerManager by turning manualControlTestMode to true in the inspector.
    /// Use UHJK to control ball.
    /// </summary>
    private void BallManualControl()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector2 moveDirection = Vector2.zero;

        if (Input.GetKey(KeyCode.U) &&
            Input.GetKey(KeyCode.J)
        )
        {
            moveDirection.y = 0;
        }
        else if (Input.GetKey(KeyCode.U))
        {
            moveDirection.y = 1;
        }
        else if (Input.GetKey(KeyCode.J))
        {
            moveDirection.y = -1;
        }

        if (Input.GetKey(KeyCode.K) &&
            Input.GetKey(KeyCode.H)
        )
        {
            moveDirection.x = 0;
        }
        else if (Input.GetKey(KeyCode.K))
        {
            moveDirection.x = 1;
        }
        else if (Input.GetKey(KeyCode.H))
        {
            moveDirection.x = -1;
        }

        if (moveDirection != Vector2.zero)
        {
            // Calculate movement amount based on move speed and time
            Vector2 movement = moveDirection * 15f;

            // Apply movement to the Rigidbody
            ballRB.AddForce(movement);
        }

        if (Input.GetKeyDown(KeyCode.Backslash))
        {
            TeleportPlayerToBall();
        }
    }

    private void GameStartProcessing()
    {
        // DontDestroyOnLoad(gameObject);
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
        var hudCanvas = GameObject.Find("HUDCanvas");

        healthBar = hudCanvas.GetComponentInChildren<HealthBar>();
        PlayerParent = GameObject.Find("Player");

        var snapToGridLocation = new Vector3();

        snapToGridLocation.x = Mathf.Floor(PlayerParent.transform.position.x);
        snapToGridLocation.y = Mathf.Floor(PlayerParent.transform.position.y);
        snapToGridLocation.z = PlayerParent.transform.position.z;

        PlayerParent.transform.position = snapToGridLocation;

        healthBar.UpdateHealthBar(currentHP, maxHP);//healthbar
        UpdateHPText();

        if (GameManager.Instance.HPText != null) GameManager.Instance.HPText.text = $"HP: {currentHP}/{maxHP}";

        playerWizard = GameObject.Find("Wizard Parent");
        playerBall = GameObject.Find("Ball Parent");
        var player = GameObject.Find("Player");
        powerLevelController = player.GetComponentInChildren<PowerLevelController>();
        varianceLevelController = player.GetComponentInChildren<VarianceLevelController>();
        inventoryController = GetComponentInChildren<InventoryController>();
        actionStateController = player.GetComponentInChildren<PlayerActionStateController>();

        ballRB = playerBall.GetComponent<Rigidbody2D>();
        wizardRB = playerWizard.GetComponent<Rigidbody2D>();

        golfAim = playerBall.GetComponentInChildren<GolfAim>();
        golfAimDrag = playerBall.GetComponentInChildren<GolfAimDrag>();
        powerLevelController = playerBall.GetComponentInChildren<PowerLevelController>();

        inventoryController.Init();
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

    public void RestoreHealth(int restore)
    {
        currentHP += restore;
        if (currentHP > maxHP) currentHP = maxHP;
        UpdateHPText();
    }

    public void UpdateHPText()
    {
        if (GameManager.Instance.HPText != null) GameManager.Instance.HPText.text = $"HP: {currentHP}/{maxHP}";
        healthBar.UpdateHealthBar(currentHP, maxHP);//healthbar
    }

    public void TeleportPlayerToBall()
    {
        //TODO this shouldn't be here but it works.
        UpdateHPText();
        healthBar.UpdateHealthBar(currentHP, maxHP);//healthbar

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

    private bool isBallInSand = false;
    public float sandDrag = 4f;

    public void ApplySandDrag()
    {
        if (!isBallInSand)
        {
            ballRB.drag += sandDrag;
            isBallInSand = true;
        }
    }

    public void RemoveSandDrag()
    {
        if (isBallInSand)
        {
            ballRB.drag -= sandDrag;
            isBallInSand = false;
        }

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

    public void CompleteLevelActions()
    {
        EndLevelRegen();
    }

    private void EndLevelRegen()
    {
        float amountToHealFloat = maxHP * endLevelRegenPercent;

        RestoreHealth((int)amountToHealFloat);
    }

    private void PlayerDies()
    {
        GameManager.Instance.GameOver();
    }

    // Level up stats
    public int EXPCurrent { get; private set; }
    public int EXPNeededToLevelUp
    {
        get
        {
            int expNeeded = 0;

            for (int i = 0; i < EXPLevel; i++)
            {
                expNeeded += (int)(level1EXP * Mathf.Pow(EXPLevel, 0.4f));
            }

            return expNeeded;
        }
    }
    private int level1EXP = 100;
    public int EXPLevel { get; private set; }
    public int EXPInCurrentLevel
    {
        get
        {
            return (int)(level1EXP * Mathf.Pow(EXPLevel, 0.4f));
        }
    }

    public int baseDamage = 0;

    public void EXPGain(int increase)
    {
        EXPCurrent += increase;

        if (EXPCurrent >= EXPNeededToLevelUp)
        {
            LevelUp();


        }
    }


    private void LevelUp()
    {
        IncreaseBaseDamage(1);
        IncreaseMaxHP(5);

        EXPLevel++;


    }

    public void IncreaseMaxHP(int increaseBy)
    {
        maxHP += increaseBy;
        currentHP += increaseBy;
    }

    public void IncreaseBaseDamage(int increaseBy)
    {
        baseDamage += increaseBy;
    }

    private void UpdateUIforEXP()
    {

    }

}
