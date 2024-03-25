using UnityEngine;

public class PowerState : IPlayerActionState
{
  PlayerActionStateController controller;

  public void OnEnter(PlayerActionStateController controller)
  {
    this.controller = controller;
    Debug.Log("Player Entered Power State");
    // TODO
    // PlayerManager.Instance.powerLevelController.ShowPowerBar();
  }

  public void OnExit()
  {
    Debug.Log("Player Exited Power State");
  }

  public void OnUpdate()
  {

  }
}