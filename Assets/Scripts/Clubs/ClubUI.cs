using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClubUI : MonoBehaviour
{
    [SerializeField] private Text ClubNameText;
   private void Start() {

    ChestController.OnClubAdded += ClubCollectUI;
    }

    private void ClubCollectUI(Club club)
    
    {
    ClubNameText.text = club.clubName;
    }


}
