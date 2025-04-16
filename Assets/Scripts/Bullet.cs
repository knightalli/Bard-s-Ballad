using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _lifetime;
    [SerializeField] private float _distance;
    [SerializeField] private int _damage;
    [SerializeField] private LayerMask whatIsSolid;

    private void Update()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.up, _distance, whatIsSolid);
        if (hitInfo.collider != null )
        {
            if (hitInfo.collider.TryGetComponent<Enemy>(out Enemy enemy))
            {
                enemy.TakeDamage(_damage);

            }
            Destroy(gameObject);
        }
        transform.Translate(Vector2.right * _speed * Time.deltaTime);
    }
}
