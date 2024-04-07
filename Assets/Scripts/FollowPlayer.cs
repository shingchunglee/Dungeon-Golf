using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    private void Update()
    {
        transform.position = new Vector3(PlayerManager.Instance.transform.position.x, PlayerManager.Instance.transform.position.y, transform.position.z);
    }
}