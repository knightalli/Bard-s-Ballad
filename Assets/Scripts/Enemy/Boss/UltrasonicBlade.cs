using UnityEngine;

// Скрипт для ультразвукового клинка
public class UltrasonicBlade : MonoBehaviour
{
    public float lifetime = 5f;
    public int damage = 15;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var other = collision.collider;
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerBhvr>()?.TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
