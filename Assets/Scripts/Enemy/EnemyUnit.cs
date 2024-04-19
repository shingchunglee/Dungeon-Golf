using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class EnemyUnit : MovingUnit
{

    public int ID { get; private set; }
    public string enemyName = "";
    public int MaxHP;
    [HideInInspector] public int CurrentHP;
    public int damage = 1;

    private EnemyManager enemyManager;
    private Rigidbody2D targetRB;

    protected void Awake()
    {
        ID = gameObject.GetInstanceID();
    }

    protected override void Start()
    {
        CurrentHP = MaxHP;
        GetComponentInChildren<Collider2D>().isTrigger = (CurrentHP == 1);

        enemyManager = GameManager.Instance.enemyManager;
        enemyManager.AddEnemyToList(this);

        targetRB = PlayerManager.Instance.playerWizard.GetComponent<Rigidbody2D>();

        base.Start();
    }

    public void TakeTurn()
    {
        Debug.Log($"Enemy '{name}', ID: {ID}, has taken its turn.");
        MoveEnemy();
    }

    //MoveEnemy is called by the GameManger each turn to tell each Enemy to try to move towards the player.
    public void MoveEnemy()
    {
        int xDir = 0;
        int yDir = 0;

        Debug.Log($"Enemy thinks player at x: {targetRB.position.x}" +
    $"Enemy thinks player at y: {targetRB.position.y}");

        //If the difference in positions is approximately zero (Epsilon) do the following:
        if (Mathf.Abs(targetRB.position.x - transform.position.x) < float.Epsilon)

            //If the y coordinate of the target's (player) position is greater than the y coordinate of this enemy's position set y direction 1 (to move up). If not, set it to -1 (to move down).
            yDir = targetRB.position.y > transform.position.y ? 1 : -1;

        //If the difference in positions is not approximately zero (Epsilon) do the following:
        else
            //Check if target x position is greater than enemy's x position, if so set x direction to 1 (move right), if not set to -1 (move left).
            xDir = targetRB.position.x > transform.position.x ? 1 : -1;

        //Call the AttemptMove function and pass in the generic parameter Player, because Enemy is moving and expecting to potentially encounter a Player
        AttemptMove<PlayerManager>(xDir, yDir);
    }

    //OnCantMove is called if Enemy attempts to move into a space occupied by a Player, it overrides the OnCantMove function of MovingObject 
    //and takes a generic parameter T which we use to pass in the component we expect to encounter, in this case Player
    protected override void OnCantMove<T>(T component)
    {
        //Declare hitPlayer and set it to equal the encountered component.
        PlayerManager hitPlayer = component as PlayerManager;

        hitPlayer.TakeDamage(damage);
    }

    void TakeDamage(int amount)
    {
        CurrentHP -= amount;

        if (CurrentHP <= 0)
        {
            EnemyDies();
        }

        else if (CurrentHP == 1)
        {
            GetComponent<Collider2D>().isTrigger = true;
        }
        else
        {
            GetComponent<Collider2D>().isTrigger = false;
        }

    }

    void EnemyDies()
    {
        enemyManager.RemoveEnemyFromList(this);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ball"))
        {
            TakeDamage(damage);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("ball"))
        {
            Destroy(gameObject);
        }
    }
}
