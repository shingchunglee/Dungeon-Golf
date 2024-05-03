using System.Collections;
using UnityEngine;

public class MoveState : IPlayerActionState
{
  PlayerActionStateController controller;

  private bool isMoving = false;

  public void OnEnter(PlayerActionStateController controller)
  {
    this.controller = controller;
    Debug.Log("Player Entered Moving State");
    // controller.ballRB.AddForce(controller.ballRB.transform.up * (float)PlayerManager.Instance.powerLevelController.selectedPower);
    if (GameManager.Instance.golfAimType == GolfAimType.Drag)
    {
      // Debug.Log("aimdirection: " + PlayerManager.Instance.golfAimDrag.aimDirection);
      controller.ballRB.AddForce((Vector2)(PlayerManager.Instance.golfAimDrag.aimDirection * (float)PlayerManager.Instance.powerLevelController.selectedPower));
    }
    else
    {
      Debug.Log("aimdirection: " + PlayerManager.Instance.golfAim.aimDirection);
      // controller.ballRB.AddForce((Vector2)(Quaternion.AngleAxis((float)PlayerManager.Instance.varianceLevelController.selectedVariance, Vector3.forward) * PlayerManager.Instance.golfAim.aimDirection * (float)PlayerManager.Instance.powerLevelController.selectedPower));
      controller.ballRB.AddForce((Vector2)(PlayerManager.Instance.golfAim.aimDirection * (float)PlayerManager.Instance.powerLevelController.selectedPower));
    }
    SoundManager.Instance.PlaySFX(SoundManager.Instance.stroke);
    // ! HACK SOLUTION: ME BRAIN NO WORK NO KNOW BETTER SOLUTION PLZ HELP
    controller.StartCoroutine(SetMoving(1f));
  }

  IEnumerator SetMoving(float delay)
  {
    yield return new WaitForSeconds(delay);
    isMoving = true;
  }

  public void OnExit()
  {
    PlayerManager.Instance.TeleportPlayerToBall();
    // Debug.Log("Player Exited Moving State");
  }

  public void OnFixedUpdate()
  {
    // This uses the player manager to check that the ball is moving.
    if (PlayerManager.Instance.IsBallMoving)
    {
      // if (GameManager.Instance.golfAimType == GolfAimType.Drag)
      // {
      //   controller.ballRB.velocity = RotateVector2(controller.ballRB.velocity, -PlayerManager.Instance.golfAimDrag.variance);
      // }
      // else
      // {
      // Debug.Log("Variance: " + (float)PlayerManager.Instance.varianceLevelController.selectedVariance);
      controller.ballRB.velocity = RotateVector2(controller.ballRB.velocity, (float)PlayerManager.Instance.varianceLevelController.selectedVariance);
      // }
      isMoving = true;
    }
    if (isMoving && !PlayerManager.Instance.IsBallMoving)
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

  public void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.TryGetComponent(out PlayerMovingOnTrigger playerMovingOnTrigger))
    {
      playerMovingOnTrigger.OnTrigger();
    }
  }

  public void OnUpdate()
  {
    if (Input.GetKeyDown(KeyCode.Period))
    {
      // Debug.Log("Force End Move State.");
      controller.SetState(controller.enemyTurnState);
    }
  }
}