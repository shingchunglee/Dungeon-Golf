using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public int MaxEnemyHP;
    public int CurrentEnemyHP;

    // Start is called before the first frame update
    void Start()
    {
        CurrentEnemyHP = MaxEnemyHP;
        GetComponent<Collider2D>().isTrigger = (CurrentEnemyHP == 1);
    }

    void dealDamage(int amount)
    {
        CurrentEnemyHP -= amount;

        if (CurrentEnemyHP <= 0)
        {
            Destroy(gameObject);
        }

        else if (CurrentEnemyHP == 1)

        {
            GetComponent<Collider2D>().isTrigger = true;
        }
        else
        {
            GetComponent<Collider2D>().isTrigger = false;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ball"))
        {
            dealDamage(1);
        }
    }


}
