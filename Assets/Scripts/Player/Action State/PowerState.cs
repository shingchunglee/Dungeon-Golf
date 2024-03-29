using System;
using UnityEngine;

public class PowerState : IPlayerActionState
{
  PlayerActionStateController controller;

  public void OnEnter(PlayerActionStateController controller)
  {
    this.controller = controller;
    Debug.Log("Player Entered Power State");
    PlayerManager.Instance.powerLevelController.ShowPowerBar();
  }

  public void OnExit()
  {
    Debug.Log("Player Exited Power State");
  }

  public void OnFixedUpdate()
  {
  }

  public void OnUpdate()
  {
    if (Input.GetMouseButtonDown(0))
    {
      OnMouseClick();
    }
  }

  private void OnMouseClick()
  {
    PlayerManager.Instance.powerLevelController.SelectPowerLevel();
    controller.SetState(controller.moveState);
    PlayerManager.Instance.golfAim.enabled = false;
  }
}