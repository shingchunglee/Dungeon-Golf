using UnityEngine;

public class BallColliderController : MonoBehaviour
{
    public PlayerActionStateController actionStateController;

    private void Start()
    {
        var player = GameObject.Find("Player");
        actionStateController = player?.GetComponentInChildren<PlayerActionStateController>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        actionStateController.OnTriggerEnter2D(other);
    }
}