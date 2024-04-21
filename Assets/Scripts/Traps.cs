using UnityEngine;

public class Traps : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Player collided with Trap");
        if (collision.gameObject.CompareTag("Trap"))
        {
            PlayerManager.Instance.TakeDamage(15);
        }
    }
}
