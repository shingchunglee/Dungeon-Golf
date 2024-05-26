using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsCanvasManager : MonoBehaviour
{
    // public ToggleGroup aimGroup;
    public Toggle Drag;
    public Toggle Click;
    public Toggle Invincibility;
    public Slider Music;
    public Slider SFX;
    public GameObject settingsPage;

    private bool isSettingsOpen = false;
    public bool pressP = false;

    private void Start()
    {

    }

    private void Update()
    {
        if (pressP && Input.GetKeyDown(KeyCode.P) && !isSettingsOpen)
        {
            openSettings();
            GameManager.Instance.isCursorOverHUDElement = true;
        }
    }

    public void openSettings()
    {
        settingsPage.SetActive(true);
        isSettingsOpen = true;
    }

    private void OnEnable()
    {
        if (SettingsManager.Instance.golfAimType == GolfAimType.Drag)
        {
            Drag.isOn = true;
        }
        else
        {
            Click.isOn = true;
        }
        Drag.onValueChanged.AddListener(OnDrag);
        Click.onValueChanged.AddListener(OnClick);

        if (PlayerManager.Instance != null)
        {
            if (PlayerManager.Instance != null && PlayerManager.Instance.isInvincible)
            {
                Invincibility.isOn = true;
            }
            else
            {
                Invincibility.isOn = false;
            }
        }

        Invincibility.onValueChanged.AddListener(IsInvincible);

        float playerPrefMusicVolume = Mathf.Clamp01(PlayerPrefs.GetFloat("MusicVolume", 1));
        Music.value = playerPrefMusicVolume;
        Music.onValueChanged.AddListener((float value) =>
        {
            SoundManager.Instance.volumeController.SetMusicVolume(value);
        });

        float playerPrefSfxVolume = Mathf.Clamp01(PlayerPrefs.GetFloat("SfxVolume", 1));
        SFX.value = playerPrefSfxVolume;
        SFX.onValueChanged.AddListener((float value) =>
        {
            SoundManager.Instance.volumeController.SetSFXVolume(value);
        });
    }

    private void OnDrag(bool isOn)
    {
        if (isOn)
        {
            SettingsManager.Instance.golfAimType = GolfAimType.Drag;
            if (PlayerManager.Instance.actionStateController.currentState == PlayerManager.Instance.actionStateController.aimState
                || PlayerManager.Instance.actionStateController.currentState == PlayerManager.Instance.actionStateController.powerState
                || PlayerManager.Instance.actionStateController.currentState == PlayerManager.Instance.actionStateController.varianceState
            )
            {
                PlayerManager.Instance.actionStateController.SetState(PlayerManager.Instance.actionStateController.aimState);
            }
        }
    }

    private void OnClick(bool isOn)
    {
        if (isOn)
        {
            SettingsManager.Instance.golfAimType = GolfAimType.Click;
            if (PlayerManager.Instance.actionStateController.currentState == PlayerManager.Instance.actionStateController.aimState
                || PlayerManager.Instance.actionStateController.currentState == PlayerManager.Instance.actionStateController.powerState
                || PlayerManager.Instance.actionStateController.currentState == PlayerManager.Instance.actionStateController.varianceState
            )
            {
                PlayerManager.Instance.actionStateController.SetState(PlayerManager.Instance.actionStateController.aimState);
            }
        }
    }

    private void IsInvincible(bool isOn)
    {
        if (isOn)
        {
            PlayerManager.Instance.SetPlayerInvincibility(true);
        }
        else
        {
            PlayerManager.Instance.SetPlayerInvincibility(false);
        }
    }

    public void CloseSettings()
    {
        // SceneManager.UnloadSceneAsync("Settings");
        settingsPage.SetActive(false);
        isSettingsOpen = false;

        if (GameManager.Instance != null)
            GameManager.Instance.isCursorOverHUDElement = false;
    }

    public void ButtonQuit()
    {
        Application.Quit();
    }

    public void ButtonMainMenu()
    {
        var GameManagerGOPlus = GameObject.Find("GameManager +");

        if (GameManagerGOPlus != null)
        {
            SceneManager.MoveGameObjectToScene(GameManagerGOPlus, SceneManager.GetActiveScene());
        }


        SceneManager.LoadScene("Main Menu");
    }
}