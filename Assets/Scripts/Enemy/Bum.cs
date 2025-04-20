using UnityEngine;

public class Bum : Enemy
{
    [SerializeField] private int _customHealth;
    [SerializeField] private float _startTimeBtwAttack;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private float _speed = 3f;
    [SerializeField] private Transform _enemyAttackPoint;
    [SerializeField] private float _enemyAttackRange = 1f;
    [SerializeField] private int _customDamage;
    [SerializeField] private LayerMask _whatIsPlayer;

    private float _timeBtwAttack;
    private SpriteRenderer _sr;

    void Start()
    {
        SetHealth(_customHealth);
        SetDamage(_customDamage);
        _timeBtwAttack = _startTimeBtwAttack;
        _sr = GetComponent<SpriteRenderer>();
    }

    protected override void Update()
    {
        base.Update();
        if (IsStunned())
            return;

        MoveTowardsPlayer();
        if (_timeBtwAttack <= 0f)
        {
            MeleeAttack();
            _timeBtwAttack = _startTimeBtwAttack;
        }
        else
        {
            _timeBtwAttack -= Time.deltaTime;
        }
    }

    private void MoveTowardsPlayer()
    {
        Vector3 direction = (_playerTransform.position - transform.position).normalized;
        transform.position += direction * _speed * Time.deltaTime;
        _sr.flipX = direction.x < 0;
    }

    private void MeleeAttack()
    {
        Collider2D hit = Physics2D.OverlapCircle(
        _enemyAttackPoint.position,
        _enemyAttackRange,
        _whatIsPlayer
    );

        if (hit != null && hit.TryGetComponent<PlayerBhvr>(out var player))
        {
            player.TakeDamage(_damage);
            print("������� ���� ������ " + _damage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (_enemyAttackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_enemyAttackPoint.position, _enemyAttackRange);
    }
}
