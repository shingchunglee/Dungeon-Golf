using UnityEngine;
using TMPro;

public class FreeCameraUIController : MonoBehaviour
{
    public CameraMove cameraController; 
    public TextMeshProUGUI spectatorModeText; 

    void Update()
    {
        
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            spectatorModeText.gameObject.SetActive(true);
        }
        else
        {
            spectatorModeText.gameObject.SetActive(false);
        }
    }
}

