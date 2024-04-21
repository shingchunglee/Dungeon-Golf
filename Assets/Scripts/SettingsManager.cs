using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    // public ToggleGroup aimGroup;
    public Toggle Drag;
    public Toggle Click;
    public GameObject settingsPage;

    private bool isSettingsOpen = false;

    private void Start()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isSettingsOpen)
        {
            settingsPage.SetActive(true);
            isSettingsOpen = true;
            GameManager.Instance.isCursorOverHUDElement = true;
        }
    }

    private void OnEnable()
    {
        if (GameManager.Instance.golfAimType == GolfAimType.Drag)
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
            GameManager.Instance.golfAimType = GolfAimType.Drag;
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
            GameManager.Instance.golfAimType = GolfAimType.Click;
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
}