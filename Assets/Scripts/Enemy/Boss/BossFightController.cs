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

    [Header("Phase Timings")]
    [SerializeField] private float timeBetweenAttacks = 2f;

    [Header("Phase 1 – Rock Storm")]
    [SerializeField] private RicochetNote ricochetNotePrefab;
    [SerializeField] private int notesPerVolley = 5;
    [SerializeField] private float noteSpeed = 8f;
    [SerializeField] private float timeBetweenNotes = 0.4f;

    [SerializeField] private UltrasonicBlade ultrasonicBladePrefab;
    [SerializeField] private float bladeSpeed = 10f;

    [Header("Phase 2 – Apotheosis")]
    [SerializeField] private Transform[] _meteorTargets;
    [SerializeField] private Meteor meteorPrefab;
    [SerializeField] private int meteorsPerRain = 6;
    [SerializeField] private float timeBeforeMeteorFalls = 1.2f;
    [SerializeField] private float timeBetweenMeteors = 0.5f;

    [SerializeField] private DaggerProjectile daggerPrefab;
    [SerializeField] private int daggersPerVolley = 3;
    [SerializeField] private float daggerSpeed = 12f;
    [SerializeField] private float timeBetweenDaggers = 0.2f;

    private Transform player;
    private bool phaseTwoStarted = false;

    private void Awake()
    {
        SetHealth(maxHealth);
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(AttackRoutine());
    }

    protected override void Update()
    {
        base.Update();
    }

    private IEnumerator AttackRoutine()
    {
        yield return new WaitForSeconds(1f);

        while (GetHealth() > 0 && !IsStunned())
        {
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
                yield return Phase2_MeteorRain();
                yield return new WaitForSeconds(timeBetweenAttacks);
                yield return Phase2_DaggerVolley();
                yield return new WaitForSeconds(timeBetweenAttacks);
            }
            //else
            //{
            //    yield return Phase2_MeteorRain();
            //    yield return new WaitForSeconds(timeBetweenAttacks);
            //    yield return Phase2_DaggerVolley();
            //    yield return new WaitForSeconds(timeBetweenAttacks);
            //}
        }

        if (GetHealth() <= 0)
            OnBossDefeated();
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
            Vector3 meteorPos = new Vector3(target.position.x, target.position.y, target.position.z);

            Instantiate(meteorPrefab, meteorPos, Quaternion.identity);
            yield return new WaitForSeconds(0.01f);
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
