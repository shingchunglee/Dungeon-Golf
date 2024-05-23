using System;
using UnityEngine;

// [Serializable]
public enum ClubType
{
    stonePutter, stoneWedge, stoneDriver, Iron7,
    ironPutter, ironWedge, ironDriver, excaliberSEdge,
    bronzePutter, bronzeWedge, bronzeDriver, baronsBalance,
    silverPutter, silverWedge, silverDriver, merlinsWhisper,
    moonironPutter, moonironWedge, moonironDriver, ironOfStarfall,
    elvensteelPutter, elvensteelWedge, elvensteelDriver, mysticsMallet,
    dragonbonePutter, dragonboneWedge, dragonboneDriver, royalRumble,
    goldPutter, goldWedge, goldDriver, whackOfWarlock,
    auroritePutter, auroriteWedge, auroriteDriver, arcaneStrike,
    mythrilPutter, mythrilWedge, mythrilDriver, LegendaryClub,
    drabDriver, plainPutter,



}

public class ClubFactory
{
    public static Club Factory(ClubType clubType)
    {
        switch (clubType)
        {
            // Basic
            case ClubType.drabDriver:
                return Resources.Load<Club>("Clubs/DrabDriver");
            case ClubType.plainPutter:
                return Resources.Load<Club>("Clubs/PlainPutter");

            //level1
            case ClubType.stonePutter:
                return Resources.Load<Club>("Clubs/stonePutter");
            case ClubType.stoneWedge:
                return Resources.Load<Club>("Clubs/stoneWedge");
            case ClubType.stoneDriver:
                return Resources.Load<Club>("Clubs/stoneDriver");
            case ClubType.Iron7:
                return Resources.Load<Club>("Clubs/7-Iron");

            //level2
            case ClubType.ironPutter:
                return Resources.Load<Club>("Clubs/IronPutter");
            case ClubType.ironWedge:
                return Resources.Load<Club>("Clubs/IronWedge");
            case ClubType.ironDriver:
                return Resources.Load<Club>("Clubs/IronDriver");
            case ClubType.excaliberSEdge:
                return Resources.Load<Club>("Clubs/excalibersEdge");


            //level3
            case ClubType.bronzePutter:
                return Resources.Load<Club>("Clubs/bronzePutter");
            case ClubType.bronzeWedge:
                return Resources.Load<Club>("Clubs/bronzeWedge");
            case ClubType.bronzeDriver:
                return Resources.Load<Club>("Clubs/bronzeDriver");
            case ClubType.baronsBalance:
                return Resources.Load<Club>("Clubs/baronsBalance");

            //level4

            case ClubType.silverPutter:
                return Resources.Load<Club>("Clubs/SilverPutter");
            case ClubType.silverWedge:
                return Resources.Load<Club>("Clubs/SilverWedge");
            case ClubType.silverDriver:
                return Resources.Load<Club>("Clubs/SilverDriver");
            case ClubType.merlinsWhisper:
                return Resources.Load<Club>("Clubs/merlinsWhisper");

            //level5
            case ClubType.moonironPutter:
                return Resources.Load<Club>("Clubs/MoonIronPutter");
            case ClubType.moonironWedge:
                return Resources.Load<Club>("Clubs/MoonIronWedge");
            case ClubType.moonironDriver:
                return Resources.Load<Club>("Clubs/MoonIronDriver");
            case ClubType.ironOfStarfall:
                return Resources.Load<Club>("Clubs/ironOfStarfall");

            //level6
            case ClubType.elvensteelPutter:
                return Resources.Load<Club>("Clubs/ElvenSteelPutter");
            case ClubType.elvensteelWedge:
                return Resources.Load<Club>("Clubs/ElvenSteelWedge");
            case ClubType.elvensteelDriver:
                return Resources.Load<Club>("Clubs/ElvenSteelDriver");
            case ClubType.mysticsMallet:
                return Resources.Load<Club>("Clubs/mysticsMallet");

            //level7
            case ClubType.dragonbonePutter:
                return Resources.Load<Club>("Clubs/DragonbonePutter");
            case ClubType.dragonboneWedge:
                return Resources.Load<Club>("Clubs/DragonboneWedge");
            case ClubType.dragonboneDriver:
                return Resources.Load<Club>("Clubs/DragonboneDriver");
            case ClubType.royalRumble:
                return Resources.Load<Club>("Clubs/royalRumble");

            //level8
            case ClubType.goldPutter:
                return Resources.Load<Club>("Clubs/GoldPutter");
            case ClubType.goldWedge:
                return Resources.Load<Club>("Clubs/GoldWedge");
            case ClubType.goldDriver:
                return Resources.Load<Club>("Clubs/GoldDriver");
            case ClubType.whackOfWarlock:
                return Resources.Load<Club>("Clubs/whackOfWarlock");

            //level9
            case ClubType.auroritePutter:
                return Resources.Load<Club>("Clubs/AuroritePutter");
            case ClubType.auroriteWedge:
                return Resources.Load<Club>("Clubs/AuroriteWedge");
            case ClubType.auroriteDriver:
                return Resources.Load<Club>("Clubs/AuroriteDriver");
            case ClubType.arcaneStrike:
                return Resources.Load<Club>("Clubs/arcaneStrike");

            //level10
            case ClubType.mythrilPutter:
                return Resources.Load<Club>("Clubs/MythrilPutter");
            case ClubType.mythrilWedge:
                return Resources.Load<Club>("Clubs/MythrilWedge");
            case ClubType.mythrilDriver:
                return Resources.Load<Club>("Clubs/MythrilDriver");
            case ClubType.LegendaryClub:
                return Resources.Load<Club>("Clubs/LegendaryClub");






















            default:
                return Resources.Load<Club>("Clubs/7-Iron");
        }
    }
}
