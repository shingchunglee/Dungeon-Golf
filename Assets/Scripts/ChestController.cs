using UnityEngine;

public class ChestController : MonoBehaviour
{
    public void OpenChest()
    {
        Debug.Log("open chest");
        ClubType club = GameManager.Instance.itemRandomiser.GetRandomisedClub();

        PlayerManager.Instance.inventoryController.AddClub(club);
    }
}