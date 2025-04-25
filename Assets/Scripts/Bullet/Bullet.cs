// Bullet.cs
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _distance;
    [SerializeField] private int _baseDamage;
    [SerializeField] private LayerMask _whatIsSolid;

    private int _damage;

    public void Setup(int extraPower)
    {
        _damage = _baseDamage + extraPower;
    }

    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, _distance, _whatIsSolid);
        if (hit.collider != null)
        {
            if (hit.collider.TryGetComponent<Enemy>(out var e))
                e.TakeDamage(_damage);
            Destroy(gameObject);
            return;
        }
        transform.Translate(Vector2.right * _speed * Time.deltaTime);
    }
}
