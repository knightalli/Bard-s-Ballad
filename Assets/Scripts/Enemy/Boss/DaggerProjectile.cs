using UnityEngine;

// Скрипт для звукового кинжала
public class DaggerProjectile : MonoBehaviour
{
    [SerializeField] private float lifetime = 5f;
    [SerializeField] private int damage = 12;

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
