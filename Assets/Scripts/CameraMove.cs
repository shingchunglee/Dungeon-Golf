using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public float moveSpeed = 5.0f; 
    public Transform golfBall;
    private bool isFreeCamera = false;

    void Update()
    {
        Vector3 move = new Vector3();
        
        if (Input.GetKey(KeyCode.W))
        {
            isFreeCamera = true;
            move.y += 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            isFreeCamera = true;
            move.y -= 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            isFreeCamera = true;
            move.x -= 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            isFreeCamera = true;
            move.x += 1;
        }

        transform.Translate(move * moveSpeed * Time.deltaTime);
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isFreeCamera = false;
            ResetCameraPosition();
        }

        if (!isFreeCamera)
        {
            transform.position = new Vector3(PlayerManager.Instance.playerBall.transform.position.x,
                PlayerManager.Instance.playerBall.transform.position.y, transform.position.z);
        }
    }

    void ResetCameraPosition()
    {
        if (golfBall != null)
        {
            transform.position = new Vector3(golfBall.position.x, golfBall.position.y, transform.position.z);
        }
    }
}

