using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public float moveSpeed = 5.0f; 
    public Transform golfBall; 

    void Update()
    {
        Vector3 move = new Vector3();
        
        if (Input.GetKey(KeyCode.W))
        {
            move.y += 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            move.y -= 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            move.x -= 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            move.x += 1;
        }

        transform.Translate(move * moveSpeed * Time.deltaTime);
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ResetCameraPosition();
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

