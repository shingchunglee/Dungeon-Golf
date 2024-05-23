using UnityEngine;

public class AimState : IPlayerActionState
{
  PlayerActionStateController controller;
  private bool validClick = false;

  public void OnEnter(PlayerActionStateController controller)
  {
    this.controller = controller;
    if (SettingsManager.Instance.golfAimType == GolfAimType.Drag)
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
    PlayerManager.Instance.SetLastShotPosition(PlayerManager.Instance.ballRB.position);
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
    if (Input.GetKeyDown(KeyCode.H))
    {
      PlayerManager.Instance.inventoryController.ConsumeConsumable();
    }
    // SCROLL UP DOWN CHANGE CLUBS
    if (Input.mouseScrollDelta.y > 0)
    {
      PlayerManager.Instance.inventoryController.GetNextClub();
    }
    else if (Input.mouseScrollDelta.y < 0)
    {
      PlayerManager.Instance.inventoryController.GetPreviousClub();
    }
    // LEFT RIGHT ARROWS CHANGE CLUBS
    if (Input.GetKeyDown(KeyCode.RightArrow))
    {
      PlayerManager.Instance.inventoryController.GetNextClub();
    }

    if (Input.GetKeyDown(KeyCode.LeftArrow))
    {
      PlayerManager.Instance.inventoryController.GetPreviousClub();
    }
    // UP DOWN ARROWS CHANGE CONSUMABLES
    if (Input.GetKeyDown(KeyCode.UpArrow))
    {
      PlayerManager.Instance.inventoryController.GetNextConsumable();
    }

    if (Input.GetKeyDown(KeyCode.DownArrow))
    {
      PlayerManager.Instance.inventoryController.GetPreviousConsumable();
    }
  }

  private void OnMouseUp()
  {
    if (SettingsManager.Instance.golfAimType == GolfAimType.Drag)
    {
      if (PlayerManager.Instance.golfAimDrag.OnMouseUp())
      {
        controller.SetState(controller.moveState);
      }
    }
  }

  private void OnMouseClick()
  {
    if (SettingsManager.Instance.golfAimType == GolfAimType.Drag)
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