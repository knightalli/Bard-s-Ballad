using System.Collections;
using UnityEngine;

// —крипт BossFightController наследует логику от Enemy
public class BossFightController : Enemy
{
    [Header("Boss Settings")]
    [SerializeField] private int maxHealth = 100;

    [Header("Phase Timings")]
    [SerializeField] private float timeBetweenAttacks = 2f;

    [Header("Phase 1 Ц Rock Storm")]
    [SerializeField] private RicochetNote ricochetNotePrefab;
    [SerializeField] private int notesPerVolley = 5;
    [SerializeField] private float noteSpeed = 8f;
    [SerializeField] private float timeBetweenNotes = 0.4f;

    [SerializeField] private UltrasonicBlade ultrasonicBladePrefab;
    [SerializeField] private int bladesPerVolley = 4;
    [SerializeField] private float bladeSpeed = 10f;

    [Header("Phase 2 Ц Apotheosis")]
    [SerializeField] private Transform meteorMarkerPrefab;
    [SerializeField] private Meteor meteorPrefab;
    [SerializeField] private int meteorsPerRain = 6;
    [SerializeField] private float meteorSpawnY = 15f;
    [SerializeField] private float timeBeforeMeteorFalls = 1.2f;
    [SerializeField] private float timeBetweenMeteors = 0.5f;

    [SerializeField] private DaggerProjectile daggerPrefab;
    [SerializeField] private int daggersPerVolley = 3;
    [SerializeField] private float daggerSpeed = 12f;
    [SerializeField] private float timeBetweenDaggers = 0.2f;

    private Transform player;
    private bool phaseTwoStarted = false;
    private Transform ricochetMarker;

    private void Awake()
    {
        // «адаЄм начальное здоровье через базовый метод
        SetHealth(maxHealth);
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        ricochetMarker = GameObject.Find("RicochetMarker").transform;
        StartCoroutine(AttackRoutine());
    }

    protected override void Update()
    {
        // ѕоддерживаем логику оглушени€ из Enemy
        base.Update();
    }

    private IEnumerator AttackRoutine()
    {
        yield return new WaitForSeconds(1f);

        while (GetHealth() > 0 && !IsStunned())
        {
            // ѕереход во вторую фазу при 50% HP
            if (!phaseTwoStarted && GetHealth() <= maxHealth / 2)
            {
                phaseTwoStarted = true;
                yield return new WaitForSeconds(1f);
            }

            if (!phaseTwoStarted)
            {
                yield return Phase1_RicochetNotes();
                yield return new WaitForSeconds(timeBetweenAttacks);
                yield return Phase1_UltrasonicBlades();
                yield return new WaitForSeconds(timeBetweenAttacks);
            }
            else
            {
                yield return Phase2_MeteorRain();
                yield return new WaitForSeconds(timeBetweenAttacks);
                yield return Phase2_DaggerVolley();
                yield return new WaitForSeconds(timeBetweenAttacks);
            }
        }

        if (GetHealth() <= 0)
            OnBossDefeated();
    }

    IEnumerator Phase1_RicochetNotes()
    {
        float spreadRadius = 2f;  // радиус рассе€ни€ вокруг игрока
        for (int i = 0; i < notesPerVolley; i++)
        {
            // точка спавна Ч маркер у босса
            Vector3 spawnPos = ricochetMarker.position;

            // целева€ точка Ч случайно в круге вокруг игрока
            Vector2 randomOffset = Random.insideUnitCircle * spreadRadius;
            Vector2 target = (Vector2)player.position + randomOffset;

            // создаЄм ноту
            RicochetNote note = Instantiate(ricochetNotePrefab, spawnPos, Quaternion.identity);
            Rigidbody2D rb = note.GetComponent<Rigidbody2D>();

            // направл€ем из spawnPos в target
            Vector2 dir = (target - (Vector2)spawnPos).normalized;
            rb.velocity = dir * noteSpeed;

            yield return new WaitForSeconds(timeBetweenNotes);
        }
    }

    private IEnumerator Phase1_UltrasonicBlades()
    {
        for (int i = 0; i < bladesPerVolley; i++)
        {
            float angle = (i % 2 == 0) ? 45f : 135f;
            var rot = Quaternion.Euler(0, 0, angle);
            var blade = Instantiate(ultrasonicBladePrefab, transform.position, rot);
            var rb = blade.GetComponent<Rigidbody2D>();
            rb.velocity = rot * Vector2.right * bladeSpeed;
        }
        yield return null;
    }

    private IEnumerator Phase2_MeteorRain()
    {
        for (int i = 0; i < meteorsPerRain; i++)
        {
            float x = player.position.x + Random.Range(-5f, 5f);
            Vector3 spawnPos = new Vector3(x, transform.position.y + meteorSpawnY, 0f);

            var marker = Instantiate(meteorMarkerPrefab, spawnPos, Quaternion.identity);
            yield return new WaitForSeconds(timeBeforeMeteorFalls);

            Instantiate(meteorPrefab, spawnPos, Quaternion.identity);
            Destroy(marker.gameObject);

            yield return new WaitForSeconds(timeBetweenMeteors);
        }
    }

    private IEnumerator Phase2_DaggerVolley()
    {
        for (int i = 0; i < daggersPerVolley; i++)
        {
            Vector2 dir = (player.position - transform.position).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            var rot = Quaternion.Euler(0, 0, angle);
            var dagger = Instantiate(daggerPrefab, transform.position, rot);
            var rb = dagger.GetComponent<Rigidbody2D>();
            rb.velocity = dir * daggerSpeed;
            yield return new WaitForSeconds(timeBetweenDaggers);
        }
    }

    private void OnBossDefeated()
    {
        Debug.Log("Boss defeated!");
        Destroy(gameObject, 1f);
    }
}
