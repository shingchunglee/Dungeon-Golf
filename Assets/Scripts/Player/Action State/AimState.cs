using System.Collections;
using UnityEngine;

public class AimState : IPlayerActionState
{
  PlayerActionStateController controller;


  public void OnEnter(PlayerActionStateController controller)
  {
    this.controller = controller;
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

  }
}