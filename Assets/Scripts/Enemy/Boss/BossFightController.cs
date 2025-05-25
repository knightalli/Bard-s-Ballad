using System.Collections;
using UnityEngine;

public class BossFightController : Enemy
{
    [Header("Boss Settings")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private Transform[] _ricochetSpawnPoints;
    [SerializeField] private Transform[] _ricochetTargetPoint;
    [SerializeField] private Transform[] bladeSpawnPoints;
    [SerializeField] private Transform[] bladeTargetPoints;
    [SerializeField] private Transform[] daggerSpawnPoints;
    [SerializeField] private Transform[] daggerTargetPoints;

    [Header("Phase Timings")]
    [SerializeField] private float timeBetweenAttacks = 2f;

    [Header("Phase 1 Rock Storm")]
    [SerializeField] private RicochetNote ricochetNotePrefab;
    [SerializeField] private float noteSpeed = 8f;
    [SerializeField] private float timeBetweenNotes = 0.4f;

    [SerializeField] private UltrasonicBlade ultrasonicBladePrefab;
    [SerializeField] private float bladeSpeed = 10f;

    [Header("Phase 2 Apotheosis")]
    [SerializeField] private Transform[] _meteorTargets;
    [SerializeField] private Meteor meteorPrefab;

    [SerializeField] private DaggerProjectile daggerPrefab;
    [SerializeField] private int daggersPerVolley = 3;
    [SerializeField] private float daggerSpeed = 12f;
    [SerializeField] private float timeBetweenDaggers = 0.2f;

    private Transform player;
    private bool phaseTwoStarted = false;
    private Coroutine attackCoroutine;
    private Coroutine healthMonitorCoroutine;

    public event System.Action BossDefeated;

    private void Awake()
    {
        SetHealth(maxHealth);
    }

    protected override void Start()
    {
        base.Start();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        healthMonitorCoroutine = StartCoroutine(MonitorHealth());
        attackCoroutine = StartCoroutine(AttackRoutine());
    }

    protected override void Update()
    {
        base.Update();

        if (GetHealth() <= 0 && !IsStunned())
        {
            OnBossDefeated();
        }
    }

    private IEnumerator MonitorHealth()
    {
        while (GetHealth() > 0)
        {
            if (!phaseTwoStarted && GetHealth() <= maxHealth / 2)
            {
                phaseTwoStarted = true;
                if (attackCoroutine != null)
                {
                    StopCoroutine(attackCoroutine);
                }
                attackCoroutine = StartCoroutine(AttackRoutine());
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
    }

    private IEnumerator AttackRoutine()
    {
        yield return new WaitForSeconds(1f);

        while (GetHealth() > 0 && !IsStunned())
        {
            yield return StartCoroutine(Phase1_RicochetNotes());
            yield return new WaitForSeconds(timeBetweenAttacks);
            yield return StartCoroutine(Phase1_UltrasonicBlades());
            yield return new WaitForSeconds(timeBetweenAttacks);
            yield return StartCoroutine(Phase2_MeteorRain());
            yield return new WaitForSeconds(timeBetweenAttacks);
            yield return StartCoroutine(Phase2_DaggerVolley());
            yield return new WaitForSeconds(timeBetweenAttacks);

        }

        if (GetHealth() <= 0)
        {
            if (healthMonitorCoroutine != null)
            {
                StopCoroutine(healthMonitorCoroutine);
            }
            OnBossDefeated();
        }
    }

    IEnumerator Phase1_RicochetNotes()
    {
        for (int i = 0; i < _ricochetSpawnPoints.Length; i++)
        {
            Vector3 spawnPos = _ricochetSpawnPoints[i].position;
            Vector2 dir = (_ricochetTargetPoint[i].position - spawnPos).normalized;

            RicochetNote note = Instantiate(ricochetNotePrefab, spawnPos, Quaternion.identity);
            Rigidbody2D rb = note.GetComponent<Rigidbody2D>();
            rb.velocity = dir * noteSpeed;

            yield return new WaitForSeconds(timeBetweenNotes);
        }
    }

    private IEnumerator Phase1_UltrasonicBlades()
    {
        for (int i = 0; i < bladeSpawnPoints.Length; i++)
        {
            Vector3 spawnPos = bladeSpawnPoints[i].position;
            Vector3 targetPos = bladeTargetPoints[i].position;

            Vector2 dir = (targetPos - spawnPos).normalized;

            var blade = Instantiate(ultrasonicBladePrefab, spawnPos, Quaternion.identity);
            var rb = blade.GetComponent<Rigidbody2D>();
            rb.velocity = dir * bladeSpeed;
        }
        yield return null;
    }

    private IEnumerator Phase2_MeteorRain()
    {
        for (int i = 0; i < _meteorTargets.Length; i++)
        {
            Transform target = _meteorTargets[i];
            Vector3 meteorPos = new Vector3(target.position.x, target.position.y, 9.913028f);

            Instantiate(meteorPrefab, meteorPos, Quaternion.identity);
            yield return new WaitForSeconds(0.01f);
        }
    }

    private IEnumerator Phase2_DaggerVolley()
    {
        int volleys = 3;

        for (int i = 0; i < volleys; i++)
        {
            int idx = Random.Range(0, daggerSpawnPoints.Length);

            Vector2 spawnPos = daggerSpawnPoints[idx].position;
            Vector2 targetPos = daggerTargetPoints[idx].position;
            Vector2 dir = (targetPos - spawnPos).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            var rot = Quaternion.Euler(0, 0, angle);

            Vector3 spawnPos3D = new Vector3(spawnPos.x, spawnPos.y, 9.913028f);

            var dagger = Instantiate(daggerPrefab, spawnPos3D, rot);
            var rb = dagger.GetComponent<Rigidbody2D>();
            rb.velocity = dir * daggerSpeed;

            yield return new WaitForSeconds(timeBetweenDaggers);
        }
    }

    private void OnBossDefeated()
    {
        BossDefeated?.Invoke();
        Destroy(gameObject, 1f);
    }

    public override void Die()
    {
        BossDefeated?.Invoke();
        base.Die();
    }

    private void OnDisable()
    {
        if (healthMonitorCoroutine != null)
        {
            StopCoroutine(healthMonitorCoroutine);
        }
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
        }
    }
}
