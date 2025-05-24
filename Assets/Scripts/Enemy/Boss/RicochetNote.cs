using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class RicochetNote : MonoBehaviour
{
    [Header("Ricochet Settings")]
    [Tooltip("������� ��� ����� ��������� (1 = ����� ���� ������)")]
    [SerializeField] private int maxBounces = 1;
    [Tooltip("�����, �� ������� �� ���������� ��������� ��������")]
    [SerializeField] private float bounceCooldown = 0.1f;
    [Tooltip("����� ����� ����")]
    [SerializeField] private float lifeTime = 10f;

    [Header("Trail Settings")]
    [SerializeField] private GameObject fireTrailPrefab;
    [SerializeField] private float trailSpawnInterval = 0.3f;

    [Header("Damage Settings")]
    [SerializeField] private int damageToPlayer = 10;

    private Rigidbody2D rb;
    private Collider2D col;
    private int bounceCount = 0;
    private float lastBounceTime = -Mathf.Infinity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        // ��������� �������� � ������� �������������
        rb.freezeRotation = true;
        rb.drag = 0f;
        rb.angularDrag = 0f;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }

    private void Start()
    {
        // ����-�����������
        Destroy(gameObject, lifeTime);
        // ��������� ����� �������� ������
        StartCoroutine(SpawnFireTrails());
    }

    private IEnumerator SpawnFireTrails()
    {
        while (true)
        {
            Instantiate(fireTrailPrefab, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(trailSpawnInterval);
        }
    }

    private IEnumerator DisableCollisionTemporarily()
    {
        col.enabled = false;
        yield return new WaitForSeconds(bounceCooldown);
        col.enabled = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 1) ���� ��� ����� � ������� ���� � ������
        if (collision.collider.TryGetComponent<PlayerBhvr>(out var player))
        {
            player.TakeDamage(damageToPlayer);
            Destroy(gameObject);
            return;
        }

        // 2) ���� ��� ����� � ����������� ��� �����������
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            // ���������� ����� �������� � �������� cooldown
            if (Time.time - lastBounceTime < bounceCooldown)
                return;

            lastBounceTime = Time.time;

            if (bounceCount < maxBounces)
            {
                bounceCount++;

                // ������� ���������
                Vector2 inVel = rb.velocity;
                Vector2 normal = collision.contacts[0].normal;
                Vector2 reflVel = Vector2.Reflect(inVel, normal).normalized * inVel.magnitude;
                rb.velocity = reflVel;

                // ������ ��������� �� �����, ����� �� ��������
                transform.position += (Vector3)normal * 0.05f;

                // ������� ��������� Collider, ����� �� ���� ���������� OnCollisionEnter
                StartCoroutine(DisableCollisionTemporarily());
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
