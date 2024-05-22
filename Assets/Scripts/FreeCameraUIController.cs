using UnityEngine;
using TMPro;

public class FreeCameraUIController : MonoBehaviour
{
    public TextMeshProUGUI spectatorModeText;
    private bool textDisplayed = false;

    void Start()
    {
        spectatorModeText.gameObject.SetActive(false);
    }

    void OnEnable()
    {
        CameraMove.OnFreeCameraModeChanged += HandleFreeCameraModeChanged;
    }

    void OnDisable()
    {
        CameraMove.OnFreeCameraModeChanged -= HandleFreeCameraModeChanged;
    }

    void HandleFreeCameraModeChanged(bool isFreeCamera)
    {
        if (isFreeCamera)
        {
            spectatorModeText.gameObject.SetActive(true);
            textDisplayed = true;
        }
        else
        {
            spectatorModeText.gameObject.SetActive(false);
            textDisplayed = false;
        }
    }
}





