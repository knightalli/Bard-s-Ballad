using System.Collections;
using UnityEngine;

// —крипт дл€ рикошет€щей ноты
public class RicochetNote : MonoBehaviour
{
    public int maxBounces = 3;
    public GameObject fireTrailPrefab;
    public float lifeTime = 10f;
    public int damageToPlayer = 10;

    private Rigidbody2D rb;
    private int bounceCount = 0;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifeTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var other = collision.collider;
        // ≈сли попали в игрока Ч наносим урон и уничтожаем ноту
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerBhvr>()?.TakeDamage(damageToPlayer);
            Destroy(gameObject);
            return;
        }

        // »наче Ч рикошет и создание огненного следа
        Instantiate(fireTrailPrefab, transform.position, Quaternion.identity);

        Vector2 normal = collision.contacts[0].normal;
        rb.velocity = Vector2.Reflect(rb.velocity, normal);

        bounceCount++;
        if (bounceCount >= maxBounces)
            Destroy(gameObject);
    }
}
