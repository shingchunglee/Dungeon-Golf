using System;
using UnityEngine;

// [Serializable]
public enum ClubType
{
  Iron7, Iron8,
  LegendaryClub
}

public class ClubFactory
{
  public static Club Factory(ClubType clubType)
  {
    switch (clubType)
    {
      case ClubType.Iron7:
        return Resources.Load<Club>("Clubs/7-Iron");
      case ClubType.Iron8:
        return Resources.Load<Club>("Clubs/8-Iron");
      case ClubType.LegendaryClub:
        return Resources.Load<Club>("Clubs/LegendaryClub");
      default:
        return Resources.Load<Club>("Clubs/7-Iron");
    }
  }
}
