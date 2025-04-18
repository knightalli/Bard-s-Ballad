using UnityEngine;

public class PlayerBhvr : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private int _health;

    private Rigidbody2D _rb;
    private SpriteRenderer _sr;
    private Vector2 _moveInput;
    private Vector2 _moveVelocity;


    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        _moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        _moveVelocity = _moveInput.normalized * _speed;
        if (Mathf.Abs(_moveInput.x) > 0.01f)
        {
            _sr.flipX = _moveInput.x < 0;
        }
    }

    private void FixedUpdate()
    {
        _rb.MovePosition(_rb.position + _moveVelocity * Time.fixedDeltaTime);
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;

        if (_health < 0)
        {
            Destroy(gameObject);
        }
    }
}
