public interface IPlayerActionState
{
    public void OnEnter(PlayerActionStateController controller);
    public void OnUpdate();
    public void OnExit();
}