using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreenManager : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        PlayerManager.Instance.UIElements = GameObject.Find("HUDCanvas").GetComponent<PlayerUIElements>();

        PlayerManager.Instance.CompleteLevelActions();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameManager.Instance.LoadNextLevel();
        }
    }
}
