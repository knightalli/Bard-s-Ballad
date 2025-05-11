using UnityEngine;

// ������ ��� ���������
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
        // ����� ��� �������
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        // ������� ���� ���� � �������
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player"))
                hit.GetComponent<PlayerBhvr>()?.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}
