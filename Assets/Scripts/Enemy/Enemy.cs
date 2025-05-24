using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected float _health;
    protected float _damage;
    protected float _stunTimer;

    private float _stunDuration = 0.5f;


    protected virtual void Update()
    {
        if (_stunTimer > 0f)
        {
            _stunTimer -= Time.deltaTime;
            return;
        }       
    }

    public void TakeDamage(float damage)
    {
        print("Нанесен урон " + damage + " здоровье " + _health);
        _health -= damage;

        _stunTimer = _stunDuration;

        if (_health <= 0)
            Destroy(gameObject);
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
}
