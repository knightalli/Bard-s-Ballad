using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _distance;
    [SerializeField] private int _damage;
    [SerializeField] private LayerMask _whatIsPlayer;
    [SerializeField] private LayerMask _whatIsWall;

    private void Update()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.up, _distance, _whatIsPlayer);
        
        if (hitInfo.collider != null)
        {
            if (hitInfo.collider.TryGetComponent<PlayerBhvr>(out PlayerBhvr player))
            {
                player.TakeDamage(_damage);
            }
            Destroy(gameObject);
        }

        RaycastHit2D hitWall = Physics2D.Raycast(transform.position, transform.right, _distance, _whatIsWall);
        if (hitWall.collider != null)
        {
            Destroy(gameObject);
            return;
        }

        transform.Translate(Vector2.right * _speed * Time.deltaTime);
    }
}
