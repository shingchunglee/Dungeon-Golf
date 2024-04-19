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
    if (GameManager.Instance.golfAimType == GolfAimType.Drag)
    {
      controller.rb.AddForce((Vector2)(PlayerManager.Instance.golfAimDrag.aimDirection * (float)PlayerManager.Instance.powerLevelController.selectedPower));
    }
    else
    {
      // controller.rb.AddForce((Vector2)(Quaternion.AngleAxis((float)PlayerManager.Instance.varianceLevelController.selectedVariance, Vector3.forward) * PlayerManager.Instance.golfAim.aimDirection * (float)PlayerManager.Instance.powerLevelController.selectedPower));
      controller.rb.AddForce((Vector2)(PlayerManager.Instance.golfAim.aimDirection * (float)PlayerManager.Instance.powerLevelController.selectedPower));
    }

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
      if (GameManager.Instance.golfAimType == GolfAimType.Drag)
      {
        controller.rb.velocity = RotateVector2(controller.rb.velocity, -PlayerManager.Instance.golfAimDrag.variance);
      }
      else
      {
        controller.rb.velocity = RotateVector2(controller.rb.velocity, (float)PlayerManager.Instance.varianceLevelController.selectedVariance);
      }
      isMoving = true;
    }
    if (isMoving && controller.ballRB.velocity.magnitude <= 0.1f)
    {
      controller.ballRB.velocity = new Vector2(0f, 0f);
      isMoving = false;
      controller.SetState(controller.enemyTurnState);
    }
  }

  Vector2 RotateVector2(Vector2 v, float degrees)
  {
    float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
    float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

    float tx = v.x;
    float ty = v.y;
    v.x = (cos * tx) - (sin * ty);
    v.y = (sin * tx) + (cos * ty);
    return v;
  }

  Vector2 RotateVector2(Vector2 v, float degrees)
  {
    float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
    float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

    float tx = v.x;
    float ty = v.y;
    v.x = (cos * tx) - (sin * ty);
    v.y = (sin * tx) + (cos * ty);
    return v;
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