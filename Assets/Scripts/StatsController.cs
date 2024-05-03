using System;
using UnityEngine;

public struct StatsController
{
  int enemiesKilled;
  int chestsOpened;

  public StatsController(
    int enemiesKilled,
    int chestsOpened
  )
  {
    this.enemiesKilled = enemiesKilled;
    this.chestsOpened = chestsOpened;
  }

  public void IncrementEnemiesKilled()
  {
    enemiesKilled++;
  }

  public void IncrementChestsOpened()
  {
    chestsOpened++;
  }

  internal string GetStatsToString()
  {
    return "Enemies Killed: " + enemiesKilled
    + "\nChests Opened: " + chestsOpened;
  }
}