using System;
using UnityEngine;

// [Serializable]
public enum ClubType
{
  Iron7, Iron8, baronsBalance, excaliberSEdge, ironOfStarfall, merlinsWhisper, mysticsMallet, whackOfWarlock, royalRumble, oraclesOddballPutter, arcaneStrike, wizardsWind,
  phantomSwing, LegendaryClub, wedgeOfSarcophagus, anubisDive, ironOfNile, boltOfZeus, aetheriusIron, zephyrSwing, ironOfTitan
}

public class ClubFactory
{
  public static Club Factory(ClubType clubType)
  {
    switch (clubType)
    {
      //level1
      case ClubType.Iron7:
        return Resources.Load<Club>("Clubs/7-Iron");
      case ClubType.baronsBalance:
        return Resources.Load<Club>("Clubs/baronsBalance");
      case ClubType.excaliberSEdge:
        return Resources.Load<Club>("Clubs/excalibersEdge");
      case ClubType.ironOfStarfall:
        return Resources.Load<Club>("Clubs/ironOfStarfall");
      //level2
      case ClubType.merlinsWhisper:
        return Resources.Load<Club>("Clubs/merlinsWhisper");
      case ClubType.mysticsMallet:
        return Resources.Load<Club>("Clubs/mysticsMallet");
      case ClubType.whackOfWarlock:
        return Resources.Load<Club>("Clubs/whackOfWarlock");
      case ClubType.royalRumble:
        return Resources.Load<Club>("Clubs/royalRumble");
      //level3
      case ClubType.oraclesOddballPutter:
        return Resources.Load<Club>("Clubs/oraclesOddballPutter");
      case ClubType.arcaneStrike:
        return Resources.Load<Club>("Clubs/arcaneStrike");
      case ClubType.wizardsWind:
        return Resources.Load<Club>("Clubs/wizardsWind");
      case ClubType.phantomSwing:
        return Resources.Load<Club>("Clubs/phantomSwing");
      //level4
      case ClubType.LegendaryClub:
        return Resources.Load<Club>("Clubs/LegendaryClub");
      case ClubType.wedgeOfSarcophagus:
        return Resources.Load<Club>("Clubs/wedgeOfSarcophagus");
      case ClubType.anubisDive:
        return Resources.Load<Club>("Clubs/anubisDive");
      case ClubType.ironOfNile:
        return Resources.Load<Club>("Clubs/ironOfNile");
      //level5
      case ClubType.ironOfTitan:
        return Resources.Load<Club>("Clubs/ironOfTitan");
      case ClubType.zephyrSwing:
        return Resources.Load<Club>("Clubs/zephyrSwing");
      case ClubType.aetheriusIron:
        return Resources.Load<Club>("Clubs/AetheriusIron");
      case ClubType.boltOfZeus:
        return Resources.Load<Club>("Clubs/boltOfZeus");


      default:
        return Resources.Load<Club>("Clubs/7-Iron");
    }
  }
}
