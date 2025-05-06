using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _distance;
    [SerializeField] private int _baseDamage;
    [SerializeField] private LayerMask _whatIsSolid;
    [SerializeField] private LayerMask _whatIsWall;

    private int _damage;

    public void Setup(int extraPower)
    {
        _damage = _baseDamage + extraPower;
    }

    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, _distance, _whatIsSolid);
        if (hit.collider != null)
        {
            if (hit.collider.TryGetComponent<Enemy>(out var e))
                e.TakeDamage(_damage);
            Destroy(gameObject);
            return;
        }

        RaycastHit2D hitWall = Physics2D.Raycast(transform.position, transform.right, _distance, _whatIsWall);
        if (hitWall.collider != null)
        {
            Destroy(gameObject);
            return;
        }

        transform.Translate(Vector3.right * _speed * Time.deltaTime, Space.Self);
    }
}
