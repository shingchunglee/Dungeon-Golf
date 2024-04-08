using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerManager : MonoBehaviour
{
  private static PlayerManager _instance;
  public int maxHP;
  public int currentHP;
  public PowerLevelController powerLevelController;
  // public PlayerActionStateController playerActionStateController;
  public GolfAim golfAim;
  public static PlayerManager Instance
  {
    get
    {
      if (_instance == null)
      {
        _instance = FindObjectOfType<PlayerManager>();
        if (_instance == null)
        {
          GameObject container = new GameObject("PlayerManager");
          _instance = container.AddComponent<PlayerManager>();
        }
      }
      return _instance;
    }
  }

  private void Awake()
  {
    if (_instance != null && _instance != this)
    {
      Destroy(this.gameObject);
    }
    else
    {
      _instance = this;
    }
    Init();
  }

  private void Init()
  {
    currentHP = maxHP;
  }

  public void TakeDamage(int damage)
  {
    currentHP -= damage;
    if (currentHP <= 0)
    {
      PlayerDies();
    }
  }

  private void PlayerDies()
  {
    GameManager.Instance.GameOver();
  }


}
