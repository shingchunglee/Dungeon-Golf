using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionStateController : MonoBehaviour
{
    private IPlayerActionState currentState;
    public IPlayerActionState aimState = new AimState();
    public IPlayerActionState powerState = new PowerState();

    void Start()
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
