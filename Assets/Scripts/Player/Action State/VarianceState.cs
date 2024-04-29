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
  }

  private void OnMouseClick()
  {
    PlayerManager.Instance.varianceLevelController.SelectVarianceLevel();
    controller.SetState(controller.moveState);
    PlayerManager.Instance.golfAim.enabled = false;
    PlayerManager.Instance.powerLevelController.DisablePowerBar();
  }
}