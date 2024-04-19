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
    // controller.rb.AddForce(controller.rb.transform.up * (float)PlayerManager.Instance.powerLevelController.selectedPower);
    controller.ballRB.AddForce((Vector2)(PlayerManager.Instance.golfAim.aimDirection * (float)PlayerManager.Instance.powerLevelController.selectedPower));
  }

  public void OnExit()
  {
    PlayerManager.Instance.TeleportPlayerToBall();
    Debug.Log("Player Exited Moving State");
  }

  public void OnFixedUpdate()
  {
    if (controller.ballRB.velocity.magnitude > 0.01f)
    {
      isMoving = true;
    }
    if (isMoving && controller.ballRB.velocity.magnitude <= 0.1f)
    {
      controller.ballRB.velocity = new Vector2(0f, 0f);
      isMoving = false;
      controller.SetState(controller.enemyTurnState);
    }
  }

  public void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.TryGetComponent(out PlayerMovingOnTrigger playerMovingOnTrigger))
    {
      playerMovingOnTrigger.OnTrigger();
    }
  }

  public void OnUpdate()
  {

  }
}