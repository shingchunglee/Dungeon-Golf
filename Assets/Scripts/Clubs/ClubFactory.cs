using UnityEngine;

public enum ClubType
{
  Iron7,
  LegendaryClub
}

public class ClubFactory : MonoBehaviour
{
  private static ClubFactory _instance;
  public static ClubFactory Instance
  {
    get
    {
      if (!_instance)
      {
        _instance = FindObjectOfType<ClubFactory>();

        if (!_instance)
        {
          var instance = new GameObject("ClubFactory")
                           .AddComponent<ClubFactory>();
          _instance = instance;
        }
      }
      return _instance;
    }
  }
  private ClubFactory() { } // Singleton

  public Club iron7;
  public Club legendaryClub;

  public Club Factory(ClubType clubType)
  {
    switch (clubType)
    {
      case ClubType.Iron7:
        return iron7;
      case ClubType.LegendaryClub:
        return legendaryClub;
      default:
        return iron7;
    }
  }
}
