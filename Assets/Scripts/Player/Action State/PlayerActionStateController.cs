using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionStateController : MonoBehaviour
{
    public Rigidbody2D rb;
    private IPlayerActionState currentState;
    public IPlayerActionState aimState = new AimState();
    public IPlayerActionState moveState = new MoveState();
    public IPlayerActionState powerState = new PowerState();

    void Start()
    {
        SetState(powerState);
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
}
