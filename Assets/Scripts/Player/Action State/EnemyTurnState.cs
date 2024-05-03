using UnityEngine;

public class EnemyTurnState : IPlayerActionState
{
  PlayerActionStateController controller;

  public void OnEnter(PlayerActionStateController controller)
  {
    this.controller = controller;
    // Debug.Log("Player Entered Enemy Turn State");

    GameManager.Instance.enemyManager.EnemyTurn();


  }
  public void OnExit()
  {
    // Debug.Log("Player Exited Enemy Turn State");
  }

  public void OnFixedUpdate()
  {
    Debug.Log("Enemy Turn Check" + !GameManager.Instance.enemyManager.areEnemiesTakingTheirTurns);
    if (!GameManager.Instance.enemyManager.areEnemiesTakingTheirTurns)
    {
      //   Debug.Log("Enemy Turn Complete");
      controller.SetState(controller.aimState);
    }
  }

  public void OnUpdate()
  {

  }

  public void Start()
  {

  }

  public void OnTriggerEnter2D(Collider2D collision) { }

}