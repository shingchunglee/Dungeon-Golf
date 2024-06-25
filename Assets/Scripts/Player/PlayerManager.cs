using System.Collections;
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
    public bool isInvincible { get; private set; } = false;

    public int currentDamage
    {
        get
        {
            int damage = baseDamage;

            damage += Mathf.FloorToInt(Instance.inventoryController.GetSelectedClub().damage);

            return damage;
        }
    }

    public PowerLevelController powerLevelController;
    public VarianceLevelController varianceLevelController;
    public InventoryController inventoryController;
    public Transform playerTransform;

    public int EXPParMultiplier = 2;

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

    public Vector2Int BallPositionOnWorldGrid
    {
        get
        {
            var position = new Vector2Int(
                Mathf.FloorToInt(ballRB.position.x),
                Mathf.FloorToInt(ballRB.position.y)
            );
            return position;
        }
    }

    public Node NodeAtBallLocation
    {
        get
        {
            return GameManager.Instance.gridManager.GetNodeByWorldPosition(BallPositionOnWorldGrid);
        }
    }
    private Vector3 lastShotPosition;

    public int voidDamage = 15;

    public PlayerActionStateController actionStateController;
    public GolfAim golfAim;
    public GolfAimDrag golfAimDrag;
    public PlayerUIElements UIElements;

    // STATUS EFFECTS
    public PlayerStatusEffect statusEffect;
    [SerializeField] GameObject statusEffectUI;
    public ParticleSystem explosionParticles;

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
            if (isMovingOveride) return true;

            if (ballRB == null)
            {
                return false;
            }

            if (ballRB.velocity.magnitude > 0.5f)
                return true;
            else return false;
        }
    }

    public bool isMovingOveride = false;

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
        StrengthPotion.OnConsume += () =>
        {
            statusEffect.Add(PlayerStatusEffect.StatusEffectType.STRENGTH, 3);
        };
        // ExplosionPotion.OnConsume += () =>
        // {
        //     Explode();
        //     if (explosionParticles != null)
        //     {
        //         explosionParticles.Play();
        //     }
        // };
    }

    public bool ballInMotion { get; private set; } = false;
    public Vector3 preMoveLocation;

    private void FixedUpdate()
    {
        if (manualControlTestMode)
        {
            BallManualControl();
        }

        if (IsBallMoving && ballInMotion == false)
        {
            ballInMotion = true;
            preMoveLocation = ballRB.transform.position;
        }

        if (ballInMotion == true && !IsBallMoving)
        {
            PostPlayerMovement();
            ballInMotion = false;
        }

    }

    private void PostPlayerMovement()
    {
        ballRB.velocity = Vector2.zero;

        TeleportPlayerToBall(preMoveLocation);

    }

    private void Explode()
    {
        // check for enemies in a circle of radius 3, damage them if found
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(ballRB.position, 3f, Physics.AllLayers);
        foreach (var hitCollider in hitColliders)
        {
            var enemy = hitCollider.GetComponentInParent<EnemyUnit>();
            if (enemy != null)
            {
                enemy.TakeDamage(5);
            }
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

        if (Input.GetKey(KeyCode.I) &&
            Input.GetKey(KeyCode.K)
        )
        {
            moveDirection.y = 0;
        }
        else if (Input.GetKey(KeyCode.I))
        {
            moveDirection.y = 1;
        }
        else if (Input.GetKey(KeyCode.K))
        {
            moveDirection.y = -1;
        }

        if (Input.GetKey(KeyCode.L) &&
            Input.GetKey(KeyCode.J)
        )
        {
            moveDirection.x = 0;
        }
        else if (Input.GetKey(KeyCode.L))
        {
            moveDirection.x = 1;
        }
        else if (Input.GetKey(KeyCode.J))
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

        SetLastShotPosition(ballRB.position);

        TeleportPlayerToBall();
    }

    public void Init()
    {
        var hudCanvas = GameObject.Find("HUDCanvas");

        UIElements = hudCanvas.GetComponentInChildren<PlayerUIElements>();
        PlayerParent = GameObject.Find("Player");

        if (PlayerParent != null)
        {
            var snapToGridLocation = new Vector3();
            snapToGridLocation.x = Mathf.Floor(PlayerParent.transform.position.x);
            snapToGridLocation.y = Mathf.Floor(PlayerParent.transform.position.y);
            snapToGridLocation.z = PlayerParent.transform.position.z;

            PlayerParent.transform.position = snapToGridLocation;
        }

        UIElements.UpdateHealthBar(currentHP, maxHP);//healthbar
        UpdateHPText();

        UIElements.UpdateHealthBar(currentHP, maxHP);

        UIElements.UpdateEXPUI(EXPCurrent, EXPLimitCurrentLevel);
        UIElements.UpdateLevelTextEverywhere(PlayerLevel);
        UIElements.UpdateBasePowerLevelTextInMenu(baseDamage.ToString());
        UIElements.UpdateDamageTotalEverywhere(currentDamage.ToString());

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

        UIElements.UpdateLevelTextEverywhere(PlayerLevel);

        inventoryController.Init();
        inventoryController.UpdateUI();

        SetLastShotPosition(ballRB.position);

        statusEffectUI = GameObject.Find("PlayerStatusEffect");
        if (statusEffectUI == null)
        {
            statusEffect = new PlayerStatusEffect(statusEffectUI);
        }
        else
        {
            statusEffect.statusEffectUI = statusEffectUI;
            statusEffect.UpdateIcons();
        }
        var explosionParticlesTransform = playerBall.transform.Find("ExplosionParticles");
        if (explosionParticlesTransform != null)
        {
            explosionParticles = explosionParticlesTransform.GetComponent<ParticleSystem>();
        }
    }

    public void TakeDamage(int damage)
    {

        if (!isInvincible) currentHP -= damage;

        SoundManager.Instance.PlaySFX(SoundManager.Instance.playerDamage);

        UpdateHPText();
        if (currentHP <= 0)
        {
            PlayerDies();
        }
    }

    /// <summary>
    /// Returns the number of Health healed.
    /// </summary>
    /// <param name="potentialAmountToHeal"></param>
    /// <returns></returns>
    public int RestoreHealth(int potentialAmountToHeal)
    {
        int amountToHeal = 0;

        int newHP = currentHP + potentialAmountToHeal;
        int overheal = newHP - maxHP;

        if (overheal <= 0) amountToHeal = potentialAmountToHeal;
        else amountToHeal = potentialAmountToHeal - overheal;

        currentHP += amountToHeal;

        //Safety check incase
        if (currentHP > maxHP) currentHP = maxHP;

        UpdateHPText();

        return amountToHeal;
    }

    public void UpdateHPText()
    {
        if (GameManager.Instance.HPText != null) GameManager.Instance.HPText.text = $"HP: {currentHP}/{maxHP}";
        UIElements.UpdateHealthBar(currentHP, maxHP);//healthbar
    }

    public void TeleportPlayerToBall()
    {
        //TODO this shouldn't be here but it works.
        UpdateHPText();
        UIElements.UpdateHealthBar(currentHP, maxHP);//healthbar

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

    public void TeleportPlayerToBall(Vector3 positionBeforeMove)
    {
        //TODO this shouldn't be here but it works.
        UpdateHPText();
        UIElements.UpdateHealthBar(currentHP, maxHP);//healthbar

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

        //If new position is not walkable
        if (!GameManager.Instance.gridManager.GetNodeByWorldPosition(BallPositionOnWorldGrid).IsWalkable())
        {
            var X = Mathf.Floor(positionBeforeMove.x);
            var Y = Mathf.Floor(positionBeforeMove.y);

            // playerRB.MovePosition(new Vector2(x, y));
            ballRB.position = new Vector2(X, Y);

            TakeDamage(5);
            return;
        }

        var x = Mathf.Floor(ballRB.position.x);
        var y = Mathf.Floor(ballRB.position.y);

        // playerRB.MovePosition(new Vector2(x, y));
        playerRB.position = new Vector2(x, y);

    }

    // public void AdjustBallPositionAfterShot()
    // {
    //     float xPos = ballRB.position.x;
    //     float yPos = ballRB.position.y;

    //     float xRemain = xPos % 1;
    //     float yRemain = yPos % 1;



    //     if (xRemain < 0.16f) xPos = Mathf.Floor(xPos) + 0.16f;
    //     if (xRemain > 0.84f) xPos = Mathf.Floor(xPos) + 0.84f; 

    //     Vector2 newPos =
    // }

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

    public void ResetToLastShotPosition()
    {
        ballRB.position = lastShotPosition;
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
        EndLevelEXPReward();
    }

    private void EndLevelRegen()
    {
        float amountToHealFloat = maxHP * endLevelRegenPercent;

        int amountHealed = RestoreHealth((int)amountToHealFloat);
        GameManager.Instance.UpdateEndLevelRegenText(amountHealed);
    }

    private void EndLevelEXPReward()
    {
        Debug.Log("End Level EXP Reward!");

        int value = GameManager.Instance.EXPRewardEndLevel;


        if (GameManager.Instance.isUnderPar())
        {
            value *= EXPParMultiplier;

            GameManager.Instance.UpdateEndLevelEXPText(value);
            EXPGain(value);
            Debug.Log(value);
        }
        else
        {
            EXPGain(value);
            Debug.Log(value);
        }
    }

    private void PlayerDies()
    {
        GameManager.Instance.GameOver();
    }

    // Level up stats
    public int EXPTotalCurrent { get; private set; }
    public int EXPTotalLimit
    {
        get
        {
            int expNeeded = 0;

            for (int i = 1; i < PlayerLevel + 1; i++)   //Loop needs to start at 1 to because player starts on level 1 not level 0
            {
                expNeeded += EXPCalculateLimit(i);
            }

            return expNeeded;
        }
    }

    public int EXPTotalLimitPreviousLevel
    {
        get
        {
            int expNeeded = 0;

            for (int i = 1; i < PlayerLevel; i++)   //Loop needs to start at 1 to because player starts on level 1 not level 0
            {
                expNeeded += EXPCalculateLimit(i);
            }

            return expNeeded;
        }
    }
    private int level1EXP = 100;
    public int PlayerLevel { get; private set; } = 1;
    public int EXPLimitCurrentLevel
    {
        get
        {
            return EXPCalculateLimit(PlayerLevel);
        }
    }

    public int EXPCurrent
    {
        get
        {
            return EXPTotalCurrent - EXPTotalLimitPreviousLevel;
        }
    }

    /// <summary>
    /// This determines how much EXP the players needs to earn to level up, depending on the level number.
    /// </summary>
    /// <param name="playerLevel"></param>
    /// <returns></returns>
    public int EXPCalculateLimit(int playerLevel)
    {
        return (int)(level1EXP * Mathf.Pow(playerLevel, 0.4f));
    }

    public int baseDamage = 0;

    public void EXPGain(int increase)
    {
        EXPTotalCurrent += increase;

        if (EXPTotalCurrent >= EXPTotalLimit)
        {
            LevelUp();
        }

        UIElements.UpdateEXPUI(EXPCurrent, EXPLimitCurrentLevel);
    }

    private void LevelUp()
    {
        SoundManager.Instance?.PlayLevelUpSFX();

        UIElements.PlayLevelUpAnimation();

        IncreaseBaseDamage(1);
        IncreaseMaxHP(5);

        PlayerLevel++;

        UIElements.UpdateLevelTextEverywhere(PlayerLevel);
        UIElements.UpdateBasePowerLevelTextInMenu(baseDamage.ToString());
        UIElements.UpdateDamageTotalEverywhere(currentDamage.ToString());
    }

    public void IncreaseMaxHP(int increaseBy)
    {
        maxHP += increaseBy;
        currentHP += increaseBy;

        UpdateHPText();
    }

    public bool IsHealthAtMax()
    {
        if (currentHP >= maxHP) return true;
        else return false;
    }

    public void IncreaseBaseDamage(int increaseBy)
    {
        baseDamage += increaseBy;
    }

    public void KeepMovementTurnGoing(float time)
    {
        StartCoroutine(MovementTurnCounter(time));
    }

    private IEnumerator MovementTurnCounter(float time)
    {
        isMovingOveride = true;
        yield return new WaitForSeconds(time);
        isMovingOveride = false;
    }

    public void SetPlayerInvincibility(bool isPlayerInvincible)
    {
        isInvincible = isPlayerInvincible;
    }

}
