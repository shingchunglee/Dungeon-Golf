using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Vector3 = UnityEngine.Vector3;
using Vector2 = UnityEngine.Vector2;

public class EnemyUnit : MonoBehaviour//: MovingUnit
{

    public float moveTime = 0.1f;       //Time it will take object to move, in seconds.
    public LayerMask blockingLayer;     //Layer on which collision will be checked.

    private BoxCollider2D boxCollider;  //The BoxCollider2D component attached to this object.
    private Rigidbody2D rb2D;           //The Rigidbody2D component attached to this object.
    private float inverseMoveTime;      //Used to make movement more efficient.

    public int ID { get; private set; }
    public string enemyName = "";
    public int MaxHP = 5;
    [HideInInspector] public int CurrentHP;
    public int attackDamage = 5;
    public bool isNonSolid = true;

    private EnemyManager enemyManager;
    private Rigidbody2D targetRB;

    private float closeEnough = 0.1f;

    protected void Awake()
    {
        ID = gameObject.GetInstanceID();
    }

    protected void Start()
    {
        CurrentHP = MaxHP;
        GetComponentInChildren<Collider2D>().isTrigger = (CurrentHP == 1);

        enemyManager = GameManager.Instance.enemyManager;
        enemyManager.AddEnemyToList(this);

        targetRB = PlayerManager.Instance.playerWizard.GetComponent<Rigidbody2D>();

        if (isNonSolid)
        {
            GetComponentInChildren<Collider2D>().isTrigger = true;
        }
        else
        {
            GetComponentInChildren<Collider2D>().isTrigger = false;
        }

        //Get a component reference to this object's BoxCollider2D
        boxCollider = GetComponentInChildren<BoxCollider2D>();

        //Get a component reference to this object's Rigidbody2D
        rb2D = GetComponent<Rigidbody2D>();

        //By storing the reciprocal of the move time we can use it by multiplying instead of dividing, this is more efficient.
        inverseMoveTime = 1f / moveTime;
    }

    public void TakeTurn()
    {
        Debug.Log($"Enemy '{name}', ID: {ID}, has taken its turn.");
        MoveEnemy();
    }

    protected bool Move(int xDir, int yDir, out RaycastHit2D hit)
    {
        //Store start position to move from, based on objects current transform position.
        Vector2 start = transform.position;

        // Calculate end position based on the direction parameters passed in when calling Move.
        Vector2 end = start + new Vector2(xDir, yDir);

        //Disable the boxCollider so that linecast doesn't hit this object's own collider.
        boxCollider.enabled = false;

        //Cast a line from start point to end point checking collision on blockingLayer.
        hit = Physics2D.Linecast(start, end, blockingLayer);

        //Re-enable boxCollider after linecast
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

    //MoveEnemy is called by the GameManger each turn to tell each Enemy to try to move towards the player.
    public void MoveEnemy()
    {
        int xDir = 0;
        int yDir = 0;

        // This makes the enemy choose randomly wether to travel in a X direction or a y direction first.
        if (UnityEngine.Random.value < 0.5f)
        {
            //If the difference in positions is approximately zero (Epsilon) do the following:
            if (Mathf.Abs(targetRB.position.x - transform.position.x) < closeEnough)

                //If the y coordinate of the target's (player) position is greater than the y coordinate of this enemy's position set y direction 1 (to move up). If not, set it to -1 (to move down).
                yDir = targetRB.position.y > transform.position.y ? 1 : -1;

            //If the difference in positions is not approximately zero (Epsilon) do the following:
            else
                //Check if target x position is greater than enemy's x position, if so set x direction to 1 (move right), if not set to -1 (move left).
                xDir = targetRB.position.x > transform.position.x ? 1 : -1;
        }
        else
        {
            if (Mathf.Abs(targetRB.position.y - transform.position.y) < closeEnough)
                xDir = targetRB.position.x > transform.position.x ? 1 : -1;
            else
                yDir = targetRB.position.y > transform.position.y ? 1 : -1;
        }

        //Call the AttemptMove function and pass in the generic parameter Player, because Enemy is moving and expecting to potentially encounter a Player
        AttemptMove(xDir, yDir);
    }

    //AttemptMove takes a generic parameter T to specify the type of component we expect our unit to interact with if blocked (Player for Enemies, Wall for Player).
    protected void AttemptMove(int xDir, int yDir)
    {
        //Hit will store whatever our linecast hits when Move is called.
        RaycastHit2D hit;

        //Set canMove to true if Move was successful, false if failed.
        bool canMove = Move(xDir, yDir, out hit);

        //Check if nothing was hit by linecast
        if (hit.transform == null)
            //If nothing was hit, return and don't execute further code.
            return;

        if (hit.collider.tag == "Wizard" &&
            !canMove)
        {
            PlayerManager.Instance.TakeDamage(attackDamage);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "ball" &&
        PlayerManager.Instance.actionStateController.CanBallCauseDamage())
            TakeDamageFromPlayer();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "ball" &&
        PlayerManager.Instance.actionStateController.CanBallCauseDamage())
            TakeDamageFromPlayer();
    }

    void TakeDamageFromPlayer()
    {
        CurrentHP -= PlayerManager.Instance.attackDamage;
        CheckIfDead();
    }

    void CheckIfDead()
    {
        if (CurrentHP <= 0)
        {
            EnemyDies();
        }
    }

    void EnemyDies()
    {
        enemyManager.RemoveEnemyFromList(this);
        Destroy(gameObject);
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
    }





}
