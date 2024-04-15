using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    private void Update()
    {
        transform.position = new Vector3(PlayerManager.Instance.playerBall.transform.position.x, PlayerManager.Instance.playerBall.transform.position.y, transform.position.z);
    }
}