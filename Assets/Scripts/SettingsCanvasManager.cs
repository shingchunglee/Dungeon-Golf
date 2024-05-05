using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsCanvasManager : MonoBehaviour
{
    // public ToggleGroup aimGroup;
    public Toggle Drag;
    public Toggle Click;
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

    public void CloseSettings()
    {
        // SceneManager.UnloadSceneAsync("Settings");
        settingsPage.SetActive(false);
        isSettingsOpen = false;
        GameManager.Instance.isCursorOverHUDElement = false;
    }

    public void ButtonQuit()
    {
        Application.Quit();
    }

    public void ButtonMainMenu()
    {
        var GameManagerGO = GameObject.Find("GameManager");
        var PlayerManagerGO = GameObject.Find("PlayerManager");

        if (GameManagerGO != null)
        {
            SceneManager.MoveGameObjectToScene(GameManagerGO, SceneManager.GetActiveScene());
        }

        if (PlayerManagerGO != null)
        {
            SceneManager.MoveGameObjectToScene(PlayerManagerGO, SceneManager.GetActiveScene());
        }

        SceneManager.LoadScene("Main Menu");
    }
}