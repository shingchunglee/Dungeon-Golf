using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class AimState : IPlayerActionState
{
  PlayerActionStateController controller;

  public void OnEnter(PlayerActionStateController controller)
  {
    this.controller = controller;
    PlayerManager.Instance.golfAim.enabled = true;
    Debug.Log("Player Entered Action State");

  }
  public void OnExit()
  {
    Debug.Log("Player Exited Aim State");
  }

  public void OnFixedUpdate()
  {
  }

  public void OnUpdate()
  {
    if (Input.GetMouseButtonDown(0))
    {
      if (!EventSystem.current.IsPointerOverGameObject())
      {
        OnMouseClick();
      }
    }
  }

  private void OnMouseClick()
  {
    PlayerManager.Instance.golfAim.SelectAimDirection();
    controller.SetState(controller.powerState);
  }
}