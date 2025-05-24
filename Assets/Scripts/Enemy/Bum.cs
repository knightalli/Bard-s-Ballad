using UnityEngine;

public class Bum : Enemy
{
    [SerializeField] private int _customHealth;
    [SerializeField] private float _startTimeBtwAttack;
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _attackRange = 1.5f;
    [SerializeField] private Transform _enemyAttackPoint;
    [SerializeField] private float _enemyAttackRange = 1f;
    [SerializeField] private int _customDamage;
    [SerializeField] private LayerMask _whatIsPlayer;

    private float _timeBtwAttack;
    private SpriteRenderer _sr;
    private Transform _playerTransform;
    private Rigidbody2D _rb;

    protected override void Start()
    {
        base.Start();
        _rb = GetComponent<Rigidbody2D>();
        
        // Находим игрока по тегу
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            _playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("Игрок не найден на сцене! Убедитесь, что у игрока установлен тег 'Player'");
        }

        SetHealth(_customHealth);
        SetDamage(_customDamage);
        _timeBtwAttack = _startTimeBtwAttack;
        _sr = GetComponent<SpriteRenderer>();
    }

    protected override void Update()
    {
        base.Update();
        
        if (_playerTransform == null)
        {
            // Пытаемся найти игрока, если ссылка потеряна
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                _playerTransform = player.transform;
            }
            return;
        }

        if (!IsStunned())
        {
            float distanceToPlayer = Vector2.Distance(transform.position, _playerTransform.position);
            if (distanceToPlayer > _attackRange)
            {
                MoveTowardsPlayer();
            }
            else
            {
                _rb.velocity = Vector2.zero;
            }
        }

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
        if (_playerTransform == null) return;

        Vector2 direction = (_playerTransform.position - transform.position).normalized;
        _rb.velocity = direction * _moveSpeed;
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
