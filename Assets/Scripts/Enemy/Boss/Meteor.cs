using UnityEngine;

// Скрипт для метеорита
public class Meteor : MonoBehaviour
{
    public float fallSpeed = 12f;
    public GameObject explosionPrefab;
    public float explosionRadius = 2f;
    public int damage = 20;

    private void Update()
    {
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Взрыв при падении
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        // Наносим урон всем в радиусе
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player"))
                hit.GetComponent<PlayerBhvr>()?.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}
