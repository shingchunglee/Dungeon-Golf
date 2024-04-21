using UnityEngine;

public class GolfBallCollide : MonoBehaviour
{
    private Vector3 startPosition;
    private Rigidbody2D rb;
    private bool onLava = false;

    public float stopSpeedThreshold = 1.25f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Lava"))
        {
            onLava = true;

        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Lava") && rb.velocity.sqrMagnitude <= stopSpeedThreshold * stopSpeedThreshold)
        {

            ResetShot();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Lava"))
        {
            onLava = false;
        }
    }

    private void ResetShot()
    {
        // Reset the ball's position to the start position
        transform.position = startPosition;
        // Clear any forces affecting the ball
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;
    }
}


