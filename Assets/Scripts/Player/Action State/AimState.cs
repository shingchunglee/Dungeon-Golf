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
    if (GameManager.Instance.golfAimType == GolfAimType.Drag)
    {
      PlayerManager.Instance.golfAimDrag.enabled = true;
    }
    else
    {
      PlayerManager.Instance.golfAim.enabled = true;
    }
    Debug.Log("Player Entered Action State");

  }
  public void OnExit()
  {
    Debug.Log("Player Exited Aim State");
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
    if (Input.GetMouseButtonUp(0))
    {
      OnMouseUp();
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