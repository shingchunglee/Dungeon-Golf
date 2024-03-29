using UnityEngine;

public class HUDElement : MonoBehaviour
{
  public void OnCursorEnter()
  {
    GameManager.Instance.isCursorOverHUDElement = true;
  }

  public void OnCursorExit()
  {
    GameManager.Instance.isCursorOverHUDElement = false;
  }
}