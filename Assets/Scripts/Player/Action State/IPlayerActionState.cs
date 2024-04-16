using UnityEngine;

public interface IPlayerActionState
{
    public void OnEnter(PlayerActionStateController controller);
    public void OnUpdate();
    public void OnExit();
    public void OnFixedUpdate();

    public void OnTriggerEnter2D(Collider2D collision);
}