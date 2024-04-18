using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ClubUI : MonoBehaviour
{
    public TextMeshProUGUI ClubNameText;
    public GameObject ClubClaimPanel;

    private void Start() {
        ChestController.OnClubAdded += ClubCollectUI;
    }



    private void ClubCollectUI(Club club) {
        
        if (ClubNameText != null && ClubClaimPanel != null)
        {
            ClubNameText.text = club.clubName;
            ClubClaimPanel.SetActive(true);  // Show the panel
            StartCoroutine(HidePanel());    // Start the coroutine to hide the panel
            Debug.Log("Updated ClubNameText to: " + club.clubName);
        }
    }



private IEnumerator HidePanel()
    {
        yield return new WaitForSeconds(1.15f);  // Wait for 1 second
        ClubClaimPanel.SetActive(false);      // Then hide the panel
    }
}
