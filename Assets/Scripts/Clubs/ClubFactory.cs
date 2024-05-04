using System;
using UnityEngine;

// [Serializable]
public enum ClubType
{
  Iron7, Iron8, baronsBalance, excaliberSEdge, ironOfStarfall, merlinsWhisper, mysticsMallet, whackOfWarlock, royalRumble,
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
      case ClubType.baronsBalance:
        return Resources.Load<Club>("Clubs/baronsBalance");
      case ClubType.excaliberSEdge:
        return Resources.Load<Club>("Clubs/excalibersEdge");
      case ClubType.ironOfStarfall:
        return Resources.Load<Club>("Clubs/ironOfStarfall");
      case ClubType.merlinsWhisper:
        return Resources.Load<Club>("Clubs/merlinsWhisper");
      case ClubType.mysticsMallet:
        return Resources.Load<Club>("Clubs/mysticsMallet");
      case ClubType.whackOfWarlock:
        return Resources.Load<Club>("Clubs/whackOfWarlock");
      case ClubType.royalRumble:
        return Resources.Load<Club>("Clubs/royalRumble");
      case ClubType.LegendaryClub:
        return Resources.Load<Club>("Clubs/LegendaryClub");
      default:
        return Resources.Load<Club>("Clubs/7-Iron");
    }
  }
}
