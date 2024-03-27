using UnityEngine;

public class GolfAim : MonoBehaviour
{
    public Transform golfBall; // Assign in inspector
    public Transform aimingIndicator; // Assign in inspector
    public float aimingDistance = 5.0f; // Maximum distance the indicator shows
    public float shootForce = 5.0f; // You can adjust this value in the Inspector

    void Update()
    {
        AimWithMouse();
        
        if (Input.GetMouseButtonDown(0)) // 0 is the left mouse button
        {
            ShootBall();
        }
    }

    void AimWithMouse()
    {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0; // Ensure there is no change in Z

        Vector2 aimDirection = (mouseWorldPosition - golfBall.position).normalized;
        aimingIndicator.position = golfBall.position + (Vector3)aimDirection * aimingDistance;
        aimingIndicator.up = aimDirection; // Orient the indicator
    }
    
    void ShootBall()
    {
        // Calculate the direction from the golf ball to the mouse position again
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
        mousePosition.z = 0; // Set z to 0 for 2D
        Vector2 direction = (mousePosition - golfBall.position).normalized;

        // Get the Rigidbody2D component of the golf ball
        Rigidbody2D rb = golfBall.GetComponent<Rigidbody2D>();

        // Apply the force to the golf ball in the direction of the aim
        rb.AddForce(direction * shootForce, ForceMode2D.Impulse);
    }
}