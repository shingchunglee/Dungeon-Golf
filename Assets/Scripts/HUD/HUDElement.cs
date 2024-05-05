using UnityEngine;

public class HUDElement : MonoBehaviour
{
  public void OnCursorEnter()
  {
    GameManager.Instance.isCursorOverHUDElement = true;
  }

  public void OnCursorExit()
  {
    if (!GameManager.Instance.isSettingsOpen && !GameManager.Instance.isInventoryOpen)
    {
      GameManager.Instance.isCursorOverHUDElement = false;
    }
  }
}