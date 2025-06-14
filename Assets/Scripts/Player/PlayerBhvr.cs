// PlayerBhvr.cs
using UnityEngine;

public class PlayerBhvr : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _dashSpeed;
    [SerializeField] private float _dashDuration;
    [SerializeField] private LayerMask _whatIsEnemy;
    [SerializeField] private PlayerStats _playerStats;

    private Rigidbody2D _rb;
    private SpriteRenderer _sr;
    private Vector2 _moveInput;
    private Vector2 _moveVelocity;
    private bool _isDashing;
    private float _dashTimeLeft;
    private float _dashCooldownTimer;
    private bool _isInvincible;
    private int _enemyLayerIndex;
    private float _hitFlashTimer;
    private const float HitFlashDuration = 0.5f;

    public bool stop = false;
    public Animator animator;
    public bool IsTalking = false;

    public static PlayerBhvr Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
        _enemyLayerIndex = Mathf.RoundToInt(Mathf.Log(_whatIsEnemy.value, 2));
    }

    void Update()
    {
        if (Time.timeScale == 0f || stop) return;

        if (!_isDashing)
        {
            _moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            _moveVelocity = _moveInput.normalized * _moveSpeed;

            animator.SetFloat("Move", Mathf.Abs(_moveInput.x) + Mathf.Abs(_moveInput.y));

            if (Mathf.Abs(_moveInput.x) > 0.01f)
                _sr.flipX = _moveInput.x < 0;
        }

        _dashCooldownTimer -= Time.unscaledDeltaTime;
        if (Input.GetKeyDown(KeyCode.Space) && _dashCooldownTimer <= 0f && !_isDashing)
            StartDash();

        if (_isDashing)
        {
            _dashTimeLeft -= Time.unscaledDeltaTime;
            if (_dashTimeLeft <= 0f)
                EndDash();
        }

        if (_hitFlashTimer > 0f)
        {
            _hitFlashTimer -= Time.deltaTime;
            if (_hitFlashTimer <= 0f)
                _sr.color = Color.white;
        }
    }

    void FixedUpdate()
    {
        if (Time.timeScale == 0f) return;

        if (_isDashing)
        {
            Vector2 dashDir = _moveInput.normalized;
            if (dashDir.sqrMagnitude < 0.01f)
                dashDir = Vector2.right * (_sr.flipX ? -1f : 1f);
            _rb.MovePosition(_rb.position + dashDir * _dashSpeed * Time.fixedDeltaTime);
        }
        else
        {
            _rb.MovePosition(_rb.position + _moveVelocity * Time.fixedDeltaTime);
        }
    }

    private void StartDash()
    {
        _isDashing = true;
        _isInvincible = true;
        _dashTimeLeft = _dashDuration;
        _dashCooldownTimer = _playerStats.currentCooldown + _dashDuration;
        _sr.color = new Color(1f, 1f, 1f, 0.5f);
        Physics2D.IgnoreLayerCollision(gameObject.layer, _enemyLayerIndex, true);
    }

    private void EndDash()
    {
        _isDashing = false;
        _isInvincible = false;
        _sr.color = Color.white;
        Physics2D.IgnoreLayerCollision(gameObject.layer, _enemyLayerIndex, false);
    }

    public void TakeDamage(float damage)
    {
        if (_isInvincible) return;
        _playerStats.TakeDamage(damage);

        _sr.color = Color.red;
        _hitFlashTimer = HitFlashDuration;

        if (_playerStats.currentHealth <= 0)
        {
            Destroy(gameObject);
            CanvasManager.Instance.ShowLoseScreen();
        }

    }

    public void StopPlayer()
    {
        stop = true;
    }

    public void StartPlayer()
    {
        stop = false;
    }
}
