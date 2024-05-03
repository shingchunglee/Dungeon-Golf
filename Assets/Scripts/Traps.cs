using UnityEngine;

public class Traps : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Trap"))
        {
            Debug.Log("Player collided with Trap");
            PlayerManager.Instance.TakeDamage(15);
        }
    }
}
