using UnityEngine;

public class GolfAim : MonoBehaviour
{
    public Transform golfBall;
    public Transform aimingIndicator;
    // public float aimingDistance = 5.0f;
    // public float shootForce = 5.0f;
    private Vector3 dragStartPos;
    private Vector3 dragEndPos;
    private bool isDragging = false;
    // public float stopThreshold = 0.1f;
    // public float minimumDragDistance = 0.5f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            dragStartPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            dragStartPos.z = 0;
            isDragging = true;
            aimingIndicator.gameObject.SetActive(true);
        }

        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            dragEndPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            dragEndPos.z = 0;
            float dragDistance = Vector3.Distance(dragStartPos, dragEndPos);
            // if (dragDistance >= minimumDragDistance)
            // {
            //     ShootBall();
            // }
            isDragging = false;
            aimingIndicator.gameObject.SetActive(false);
        }

        if (isDragging)
        {
            AimWithMouse();
        }

        // StopBallIfNeeded();
    }

    void AimWithMouse()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        Vector2 aimDirection = mousePosition - dragStartPos;

        aimingIndicator.position = dragStartPos + (Vector3)aimDirection;

        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;

        aimingIndicator.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    // void ShootBall()
    // {
    //     Rigidbody2D rb = golfBall.GetComponent<Rigidbody2D>();
    //     if (rb == null)
    //     {
    //         Debug.LogError("Rigidbody2D component missing from golf ball!");
    //         return;
    //     }

    //     Vector2 shootDirection = dragEndPos - dragStartPos;
    //     float dragDistance = shootDirection.magnitude;

    //     rb.AddForce(-shootDirection.normalized * dragDistance * shootForce, ForceMode2D.Impulse);

    //     aimingIndicator.gameObject.SetActive(false);
    // }

    // void StopBallIfNeeded()
    // {
    //     Rigidbody2D rb = golfBall.GetComponent<Rigidbody2D>();

    //     if (rb.velocity.magnitude < stopThreshold)
    //     {
    //         rb.velocity = Vector2.zero;
    //         rb.angularVelocity = 0f;
    //     }
    // }
}