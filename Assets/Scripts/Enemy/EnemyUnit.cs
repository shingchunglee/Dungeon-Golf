using System.Collections;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using Vector2 = UnityEngine.Vector2;
using System.Collections.Generic;

public class EnemyUnit : MonoBehaviour
{
    private float moveTime = 0.01f;           //Time it will take object to move, in seconds.

    private BoxCollider2D boxCollider;      //The BoxCollider2D component attached to this object.
    private Rigidbody2D rb2D;               //The Rigidbody2D component attached to this object.
    private float inverseMoveTime;          //Used to make movement more efficient.

    public int ID { get; private set; }
    public string enemyName = "";
    public int MaxHP = 5;
    [HideInInspector]
    public int CurrentHP;
    public int moveAttacksPerTurn = 1;      //This is how many times the unit will move AND attack the player if it is next to it.
    private int moveAttacksPerTurnLeft;
    public int maxAttacksPerTurn = 1;
    private int attacksLeft;
    public int attackDamage = 5;
    public bool willBallCollide = true;

    private EnemyManager enemyManager;
    private Rigidbody2D targetRB;
    private GameObject SpriteObj;

    private float closeEnough = 0.1f;

    [SerializeField] HealthBar healthBar;
    // [SerializeField] ParticleSystem particleEffect;
    [SerializeField] Animator enemyHurt;
    [SerializeField] Animator enemyDie;

    private float attackMoveDistance = 0.4f;
    private float attackMoveTime = 0.07f;

    protected List<Vector2Int> pathDirections;
    [HideInInspector]
    public bool isTakingTurn = false;

    public Vector2Int PositionOnWorldGrid
    {
        get
        {
            var position = new Vector2Int(
                Mathf.FloorToInt(rb2D.position.x),
                Mathf.FloorToInt(rb2D.position.y)
            );
            return position;
        }
    }

    public Node nodeAtLocation
    {
        get
        {
            return GameManager.Instance.gridManager.GetNodeByWorldPosition(PositionOnWorldGrid);
        }
    }

    protected virtual void Awake()
    {
        ID = gameObject.GetInstanceID();

        //healthbar
        healthBar = GetComponentInChildren<HealthBar>();
        if (healthBar == null)
        {
            Debug.LogError("HealthBar component not found on " + gameObject.name);
        }

        //particle system
        //  particleEffect = GetComponentInChildren<ParticleSystem>();
        // if (particleEffect == null)
        // {
        //     Debug.LogError("No Particle System found on " + gameObject.name);
        // }

        if (enemyHurt == null)
        {
            enemyHurt = GetComponentInChildren<Animator>();
        }
    }

    protected virtual void Start()
    {


        CurrentHP = MaxHP;
        // GetComponentInChildren<Collider2D>().isTrigger = (CurrentHP == 1);
        healthBar.UpdateHealthBar(CurrentHP, MaxHP);//healthbar
        enemyManager = GameManager.Instance.enemyManager;
        enemyManager.AddEnemyToList(this);

        targetRB = PlayerManager.Instance.playerWizard.GetComponent<Rigidbody2D>();

        if (willBallCollide)
        {
            GetComponentInChildren<Collider2D>().isTrigger = true;
        }
        else
        {
            GetComponentInChildren<Collider2D>().isTrigger = false;
        }

        //Set the variables
        boxCollider = GetComponentInChildren<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
        SpriteObj = this.transform.Find("Sprite").gameObject;

        SnapToGrid();

        //By storing the reciprocal of the move time we can use it by multiplying instead of dividing, this is more efficient.
        inverseMoveTime = 1f / moveTime;
    }

    private void SnapToGrid()
    {
        Vector2 pos;

        pos.x = Mathf.FloorToInt(transform.position.x);
        pos.y = Mathf.FloorToInt(transform.position.y);

        transform.position = pos;
    }

    // This is called by Enemy Manager for each enemy on the scene.
    public virtual void TakeTurn()
    {
        isTakingTurn = true;
        // Debug.Log($"Enemy '{name}', ID: {ID}, has taken its turn.");
        PreMove();
    }

    //PreMove is called by TakeTurn. This sets up the variables for a move/attack.
    protected void PreMove()
    {
        pathDirections = CalculatePathAStar();

        if (pathDirections == null) return;

        moveAttacksPerTurnLeft = moveAttacksPerTurn;
        attacksLeft = maxAttacksPerTurn;

        MoveLoop();
    }

    protected List<Vector2Int> CalculatePathAStar()
    {
        var thisEnemyNode = GameManager.Instance.gridManager.GetNodeByWorldPosition(PositionOnWorldGrid);
        var playerNode = GameManager.Instance.gridManager.GetNodeByWorldPosition(PlayerManager.Instance.WizardWorldPositionOnGrid);

        List<Node> nodePath = GridManager.FindPath(thisEnemyNode, playerNode, true);

        if (nodePath == null)
        {
            nodePath = GridManager.FindPath(thisEnemyNode, playerNode, false);

            if (nodePath == null)
            {
                return null;
            }
        }

        // Show the node path with markers
        // GameManager.Instance.gridManager.DrawPathIndicators(nodePath);

        List<Vector2Int> pathDirs = GetPathDirections(nodePath);

        return pathDirs;
    }

    protected void MoveLoop()
    {
        if (moveAttacksPerTurnLeft <= 0 ||
            attacksLeft <= 0)
        {
            PostMove();
            EndTurn();
            return;
        }
        else
        {
            moveAttacksPerTurnLeft--;
        }

        if (pathDirections.Count <= 0) return;

        var direction = pathDirections[0];

        MoveOrAttack(direction.x, direction.y);

        if (pathDirections.Count > 1)
            pathDirections.RemoveAt(0);
    }

    //AttemptMove takes a generic parameter T to specify the type of component we expect our unit to interact with if blocked (Player for Enemies, Wall for Player).
    protected void MoveOrAttack(int xDir, int yDir)
    {
        //Hit will store whatever our linecast hits when Move is called.
        RaycastHit2D hit;

        //Set canMove to true if Move was successful, false if failed.
        bool canMove = AttemptMove(xDir, yDir, out hit);

        //Check if nothing was hit by linecast
        if (hit.transform == null)
            //If nothing was hit, return and don't execute further code.
            return;

        if (hit.collider.tag == "Wizard" &&
            !canMove)
        {

            if (attacksLeft > 0)
            {

                PreAttack();
                AttackPlayer(xDir, yDir);
                PostAttack();

                attacksLeft--;
            }
        }
    }

    protected bool AttemptMove(int xDir, int yDir, out RaycastHit2D hit)
    {

        Vector2 start = transform.position; //+0.5f to make it start in the middle of the square.

        //+0.5f to make the Linecast start in the middle of the square.
        Vector2 startLine = new Vector2(transform.position.x + 0.5f, transform.position.y + 0.5f);

        // Calculate end position based on the direction parameters passed in when calling Move.
        Vector2 end = new Vector2(start.x + xDir, start.y + yDir);

        Vector2 endLine = new Vector2(startLine.x + xDir, startLine.y + yDir);

        //Disable the boxCollider so that linecast doesn't hit this object's own collider.
        boxCollider.enabled = false;

        //Cast a line from start point to end point checking collision on blockingLayer.
        hit = Physics2D.Linecast(startLine, endLine, LayerMask.GetMask("Default", "PlayerModel"));

        boxCollider.enabled = true;

        //Check if anything was hit
        if (hit.transform == null)
        {
            //If nothing was hit, start SmoothMovement co-routine passing in the Vector2 end as destination
            StartCoroutine(SmoothMovement(end));

            //Return true to say that Move was successful
            return true;
        }

        //If something was hit, return false, Move was unsuccesful.
        return false;
    }

    protected List<Vector2Int> GetPathDirections(List<Node> nodes)
    {
        var directions = new List<Vector2Int>();

        if (nodes.Count < 2)
        {
            Debug.LogError("Node list is less than 2. Aborting pathfind.");
            return directions;
        }

        for (int i = 0; i < nodes.Count - 1; i++)
        {
            var node1 = nodes[i];
            var node2 = nodes[i + 1];

            var stepDirection = node2.position - node1.position;

            directions.Add(stepDirection);
        }

        return directions;
    }

    /// <summary>
    /// This can be overridden when creating a custom enemy class inhereting EnemyUnit.
    /// This allows you to program your own enemy attacks.
    /// </summary>
    /// <param name="xDir">This is needed for the attack animation. </param>
    /// <param name="yDir">This is needed for the attack animation. </param>
    protected virtual void AttackPlayer(int xDir, int yDir)
    {
        PlayerManager.Instance.TakeDamage(attackDamage);
        StartCoroutine(AttackAnimation(xDir, yDir));
    }

    /// <summary>
    /// Attack player without animation.
    /// </summary>
    protected virtual void AttackPlayer()
    {
        PlayerManager.Instance.TakeDamage(attackDamage);
    }

    protected virtual void PreAttack()
    {

    }

    protected virtual void PostAttack()
    {

    }

    protected virtual void PostMove()
    {
        nodeAtLocation.entitiesOnTile.Add(EntityType.ENEMY);
    }

    protected virtual IEnumerator AttackAnimation(int xDir, int yDir)
    {
        var initialPos = SpriteObj.transform.position;
        var targetPos = new Vector3(
            initialPos.x + xDir * attackMoveDistance,
            initialPos.y + yDir * attackMoveDistance, 0);

        //Move out
        float elapsedTime = 0f;

        while (elapsedTime < attackMoveTime)
        {
            SpriteObj.transform.position = Vector3.Lerp(initialPos, targetPos, elapsedTime / attackMoveTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        //Move back
        elapsedTime = 0f;
        while (elapsedTime < attackMoveTime)
        {
            SpriteObj.transform.position = Vector3.Lerp(targetPos, initialPos, elapsedTime / attackMoveTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        //Loops movement so that for enemies with multiple move/attack turns.
        MoveLoop();
    }

    protected void EndTurn()
    {
        isTakingTurn = false;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "ball" &&
        PlayerManager.Instance.actionStateController.CanBallCauseDamage())
            TakeDamageFromPlayer();
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "ball" &&
        PlayerManager.Instance.actionStateController.CanBallCauseDamage())
            TakeDamageFromPlayer();
    }

    protected virtual void TakeDamageFromPlayer()
    {
        CurrentHP -= PlayerManager.Instance.attackDamage;
        healthBar.UpdateHealthBar(CurrentHP, MaxHP);//healthbar
        SoundManager.Instance.PlaySFX(SoundManager.Instance.enemyDamage);
        //     if (particleEffect != null)
        //     {
        //    //  particleEffect.Stop(); // Stop to clear any ongoing effects
        //         particleEffect.Play();
        //     }

        if (enemyHurt != null)
        {
            StopCoroutine(ShowEnemyHurt());
            StartCoroutine(ShowEnemyHurt());
        }

        CheckIfDead();
    }

    protected void CheckIfDead()
    {
        if (CurrentHP <= 0)
        {
            EnemyDies();
        }
    }

    protected void EnemyDies()
    {
        GameManager.Instance.statsController.IncrementEnemiesKilled();
        StartCoroutine(enemyDeathAnim());
    }

    //Co-routine for moving units from one space to next, takes a parameter end to specify where to move to.
    protected IEnumerator SmoothMovement(Vector3 end)
    {
        //Calculate the remaining distance to move based on the square magnitude of the difference between current position and end parameter. 
        //Square magnitude is used instead of magnitude because it's computationally cheaper.
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

        //While that distance is greater than a very small amount (Epsilon, almost zero):
        while (sqrRemainingDistance > float.Epsilon)
        {
            //Find a new position proportionally closer to the end, based on the moveTime
            Vector3 newPostion = Vector3.MoveTowards(rb2D.position, end, inverseMoveTime * Time.deltaTime);

            //Call MovePosition on attached Rigidbody2D and move it to the calculated position.
            rb2D.MovePosition(newPostion);

            //Recalculate the remaining distance after moving.
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;

            //Return and loop until sqrRemainingDistance is close enough to zero to end the function
            yield return null;
        }

        //Loops movement so that for enemies with multiple move/attack turns.
        MoveLoop();
    }

    private IEnumerator ShowEnemyHurt()
    {
        enemyHurt.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.4f);
        enemyHurt.gameObject.SetActive(false);
    }

    private IEnumerator enemyDeathAnim()
    {
        enemyDie.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        enemyDie.gameObject.SetActive(false);

        enemyManager.RemoveEnemyFromList(this);
        Destroy(gameObject);
    }
}
