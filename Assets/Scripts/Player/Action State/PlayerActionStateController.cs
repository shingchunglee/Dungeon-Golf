using UnityEngine;

public class PlayerActionStateController : MonoBehaviour
{
    public Rigidbody2D ballRB;
    public Collider2D ballCollider;
    public IPlayerActionState currentState;
    public IPlayerActionState aimState = new AimState();
    public IPlayerActionState moveState = new MoveState();
    public IPlayerActionState powerState = new PowerState();
    public IPlayerActionState varianceState = new VarianceState();
    public IPlayerActionState enemyTurnState = new EnemyTurnState();

    void Start()
    {
        ballCollider = ballRB.GetComponentInChildren<Collider2D>();

        Invoke(nameof(LateStart), 0.2f);
    }

    private void LateStart()
    {
        SetState(aimState);
    }

    void Update()
    {
        if (currentState != null)
        {
            currentState.OnUpdate();
        }
    }

    void FixedUpdate()
    {
        if (currentState != null)
        {
            currentState.OnFixedUpdate();
        }
    }

    public void SetState(IPlayerActionState newState)
    {
        if (currentState != null)
        {
            currentState.OnExit();
        }

        currentState = newState;

        currentState.OnEnter(this);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        // Debug.Log(other.name);
        currentState?.OnTriggerEnter2D(other);
    }

    public bool CanBallCauseDamage()
    {
        if (currentState == moveState)
        {
            return true;
        }
        else return false;
    }

    public void TurnOffCollider()
    {
        ballCollider.enabled = false;
    }

    public void TurnOnCollider()
    {
        ballCollider.enabled = true;
    }
}
