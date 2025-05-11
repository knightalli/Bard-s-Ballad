using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class RicochetNote : MonoBehaviour
{
    [Header("Ricochet Settings")]
    [Tooltip("Сколько раз может отскочить (1 = ровно один отскок)")]
    [SerializeField] private int maxBounces = 1;
    [Tooltip("Время, на которое мы игнорируем повторные коллизии")]
    [SerializeField] private float bounceCooldown = 0.1f;
    [Tooltip("Время жизни ноты")]
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

        // Запрещаем вращение и убираем сопротивление
        rb.freezeRotation = true;
        rb.drag = 0f;
        rb.angularDrag = 0f;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }

    private void Start()
    {
        // Авто-уничтожение
        Destroy(gameObject, lifeTime);
        // Запускаем спавн огненных следов
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
        // 1) Если это игрок — наносим урон и уходим
        if (collision.collider.TryGetComponent<PlayerBhvr>(out var player))
        {
            player.TakeDamage(damageToPlayer);
            Destroy(gameObject);
            return;
        }

        // 2) Если это стена — отскакиваем или разрушаемся
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            // Игнорируем любые коллизии в пределах cooldown
            if (Time.time - lastBounceTime < bounceCooldown)
                return;

            lastBounceTime = Time.time;

            if (bounceCount < maxBounces)
            {
                bounceCount++;

                // Считаем отражение
                Vector2 inVel = rb.velocity;
                Vector2 normal = collision.contacts[0].normal;
                Vector2 reflVel = Vector2.Reflect(inVel, normal).normalized * inVel.magnitude;
                rb.velocity = reflVel;

                // Слегка отодвинем от стены, чтобы не залипать
                transform.position += (Vector3)normal * 0.05f;

                // Коротко отключаем Collider, чтобы не было повторного OnCollisionEnter
                StartCoroutine(DisableCollisionTemporarily());
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
