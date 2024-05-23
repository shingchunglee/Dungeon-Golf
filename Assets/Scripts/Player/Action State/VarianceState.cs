using UnityEngine;

public class VarianceState : IPlayerActionState
{
  PlayerActionStateController controller;

  public void OnEnter(PlayerActionStateController controller)
  {
    this.controller = controller;
    // Debug.Log("Player Entered Variance State");
    PlayerManager.Instance.varianceLevelController.ShowVarianceBar();
  }

  public void OnExit()
  {
    // Debug.Log("Player Exited Variance State");
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
      }
    }
    // if (Input.GetMouseButtonDown(1))
    // {
    //   OnMouseRightClick();
    // }
    if (Input.GetKeyDown(KeyCode.H))
    {
      PlayerManager.Instance.inventoryController.ConsumeConsumable();
    }
  }

  private void OnMouseRightClick()
  {
    PlayerManager.Instance.varianceLevelController.DisableVarianceBar();
    PlayerManager.Instance.golfAim.enabled = false;
    PlayerManager.Instance.powerLevelController.DisablePowerBar();
    controller.SetState(controller.aimState);
  }

  private void OnMouseClick()
  {
    PlayerManager.Instance.varianceLevelController.SelectVarianceLevel();
    PlayerManager.Instance.golfAim.enabled = false;
    PlayerManager.Instance.powerLevelController.DisablePowerBar();
    controller.SetState(controller.moveState);
  }
}