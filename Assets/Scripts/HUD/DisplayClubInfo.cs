using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayClubInfo : MonoBehaviour
{
    public GameObject clubSlotPrefab;
    public Transform contentPanel;

    void Start()
    {
        PopulateClubList();
    }

    void PopulateClubList()
    {
        // Club[] clubs = Resources.LoadAll<Club>("Clubs");
        InventoryClub[] clubs = PlayerManager.Instance.inventoryController.GetInventoryClubs();
        Debug.Log("Loaded " + clubs.Length + " clubs.");

        if (clubs.Length == 0)
        {
            Debug.LogError("No clubs loaded");
            return;
        }

        foreach (var club in clubs)
        {
            GameObject newSlot = Instantiate(clubSlotPrefab, contentPanel);
            if (newSlot == null)
            {
                Debug.LogError("noclub slot prefab");
                continue;
            }

               TextMeshProUGUI text = newSlot.GetComponentInChildren<TextMeshProUGUI>();
            if (text == null)
            {
                Debug.LogError("TMP not there on prefab");
                continue;
            }

            text.text = $"{club.Club.clubName}\nDamage: {club.Club.damage}\nMax Power: {club.Club.maxPower}";
        }
    }
}