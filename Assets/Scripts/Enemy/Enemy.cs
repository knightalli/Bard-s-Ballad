using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected float _health;
    protected float _damage;
    protected float _stunTimer;

    private float _stunDuration = 0.5f;
    protected SpriteRenderer sr;

    private Room currentRoom;

    protected virtual void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Room room = other.GetComponent<Room>();
        if (room != null)
        {
            currentRoom = room;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Room room = other.GetComponent<Room>();
        if (room != null && room == currentRoom)
        {
            currentRoom = null;
        }
    }

    protected virtual void Update()
    {
        if (_stunTimer > 0f)
        {
            _stunTimer -= Time.deltaTime;
            if (_stunTimer <= 0f)
            {
                if (sr != null)
                    sr.color = Color.white;
            }
            return;
        }       
    }

    public virtual void TakeDamage(float damage)
    {
        _health -= damage;
        if (sr != null)
            sr.color = Color.red;

        _stunTimer = _stunDuration;

        if (_health <= 0)
            Die();
    }

    protected bool IsStunned() => _stunTimer > 0f;

    public void SetHealth(int value)
    {
        _health = value;
    }

    public void SetDamage(int value)
    {
        _damage = value;
    }

    public float GetHealth() => _health;

    public float GetDamage() => _damage;

    public void SetCurrentRoom(Room room)
    {
        currentRoom = room;
    }

    public virtual void Die()
    {
        if (currentRoom != null)
        {
            currentRoom.OnEnemyDeath(gameObject);
        }
        Destroy(gameObject);
    }
}
