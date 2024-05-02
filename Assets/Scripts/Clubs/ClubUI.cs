using System.Collections;
using UnityEngine;
using TMPro;

public class ClubUI : MonoBehaviour
{
    public TextMeshProUGUI ClubNameText;
    public GameObject ClubClaimPanel;
    SoundManager soundManager;

    private void Awake()
    {
        GameObject audioGameObject = GameObject.FindWithTag("Audio");
        soundManager = audioGameObject?.GetComponent<SoundManager>();
    }


    private void Start()
    {
        ChestController.OnClubAdded += ClubCollectUI;
    }



    private void ClubCollectUI(Club club)
    {

        if (ClubNameText != null && ClubClaimPanel != null)
        {
            ClubNameText.text = (club.clubName + "!");
            ClubClaimPanel.SetActive(true);
            soundManager?.PlaySFX(soundManager.clubCollect);
            StartCoroutine(HidePanel());
            Debug.Log("Updated ClubNameText to: " + club.clubName);
        }
    }



    private IEnumerator HidePanel()
    {
        yield return new WaitForSeconds(1.15f);
        ClubClaimPanel.SetActive(false);
    }
}
