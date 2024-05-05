using System.Collections;
using UnityEngine;
using TMPro;

public class ClubUI : MonoBehaviour
{
    public TextMeshProUGUI ClubNameText;
    public TextMeshProUGUI ConsumableText;
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
        ChestController.OnConsumableAdded += ConsumableCollectUI;
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

    private void ConsumableCollectUI(Consumables consumable)
    {
        if (ConsumableText != null && ClubClaimPanel != null)
        {
           ConsumableText.text = $"{consumable}"; 
            ClubClaimPanel.SetActive(true); 
            soundManager?.PlaySFX(soundManager.clubCollect);
            StartCoroutine(HidePanel());
            Debug.Log("Updated ConsumableText to: " + consumable);
        }
    }



    private IEnumerator HidePanel()
    {
        yield return new WaitForSeconds(1.8f);
        ClubClaimPanel.SetActive(false);
    }
}
