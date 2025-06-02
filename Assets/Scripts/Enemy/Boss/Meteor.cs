using UnityEngine;

public class Meteor : MonoBehaviour
{
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private float explosionRadius = 2f;
    [SerializeField] private int damage = 20;
    [SerializeField] private float timeBeforeExplosion = 0.7f;

    private void Start()
    {
        Invoke(nameof(Explode), timeBeforeExplosion);
    }

    public void Explode()
    {
        var pos = new Vector3(transform.position.x, transform.position.y, 5);
        Instantiate(explosionPrefab, pos, Quaternion.identity);

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player"))
                hit.GetComponent<PlayerBhvr>()?.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.4f);
        Gizmos.DrawSphere(transform.position, explosionRadius);
    }

}
