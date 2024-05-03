using UnityEngine;

public class AimState : IPlayerActionState
{
  PlayerActionStateController controller;
  private bool validClick = false;

  public void OnEnter(PlayerActionStateController controller)
  {
    this.controller = controller;
    if (GameManager.Instance.golfAimType == GolfAimType.Drag)
    {
      PlayerManager.Instance.golfAimDrag.enabled = true;
      PlayerManager.Instance.golfAim.enabled = false;
    }
    else
    {
      PlayerManager.Instance.golfAim.enabled = true;
      PlayerManager.Instance.golfAimDrag.enabled = false;
    }
    validClick = false;
    // Debug.Log("Player Entered Action State");

  }
  public void OnExit()
  {
    // Debug.Log("Player Exited Aim State");
  }

  public void OnFixedUpdate()
  {
  }

  public void OnTriggerEnter2D(Collider2D collision)
  {
  }

  public void OnUpdate()
  {
    if (Input.GetMouseButtonDown(0))
    {
      if (!GameManager.Instance.isCursorOverHUDElement)
      {
        OnMouseClick();
        validClick = true;
      }
    }
    if (Input.GetMouseButtonUp(0))
    {
      if (validClick)
      {
        OnMouseUp();
      }
    }

    if (Input.GetKeyDown(KeyCode.Period))
    {
      Debug.Log("Turn skipped.");
      controller.SetState(controller.enemyTurnState);
    }
  }

  private void OnMouseUp()
  {
    if (GameManager.Instance.golfAimType == GolfAimType.Drag)
    {
      PlayerManager.Instance.golfAimDrag.OnMouseUp();
      controller.SetState(controller.moveState);
    }
  }

  private void OnMouseClick()
  {
    if (GameManager.Instance.golfAimType == GolfAimType.Drag)
    {
      PlayerManager.Instance.golfAimDrag.OnMouseDown();
    }
    else
    {
      PlayerManager.Instance.golfAim.SelectAimDirection();
      controller.SetState(controller.powerState);
    }
  }
}