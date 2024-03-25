using System;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class MoveState : IPlayerActionState
{
  PlayerActionStateController controller;

  private bool isMoving = false;

  public void OnEnter(PlayerActionStateController controller)
  {
    this.controller = controller;
    Debug.Log("Player Entered Moving State");
    controller.rb.AddForce(controller.rb.transform.up * (float)PlayerManager.Instance.powerLevelController.selectedPower);
  }

  public void OnExit()
  {
    Debug.Log("Player Exited Moving State");
  }

  public void OnFixedUpdate()
  {
    if (controller.rb.velocity.magnitude > 0.01f)
    {
      isMoving = true;
    }
    if (isMoving && controller.rb.velocity.magnitude <= 0.01f)
    {
      controller.rb.velocity = new Vector2(0f, 0f);
      isMoving = false;
      controller.SetState(controller.powerState);
    }
  }

  public void OnUpdate()
  {

  }
}