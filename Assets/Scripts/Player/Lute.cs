using UnityEngine;

public class Lute : MonoBehaviour
{
    [SerializeField] private float _offset;
    [SerializeField] private GameObject _bullet;
    [SerializeField] private Transform _shotPoint;    
    [SerializeField] private float _startTimeBtwShots;

    [SerializeField] private Transform _kickPos;
    [SerializeField] private float _startTimeBtwKicks;
    [SerializeField] private LayerMask _solid;
    [SerializeField] private float _kickRange;
    [SerializeField] private int _kickDamage;

    private float _timeBtwShots;
    private float _timeBtwKicks;

    void Update()
    {
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ + _offset);

        if (_timeBtwShots <= 0)
        {
            if (Input.GetMouseButton(0))
            {
                Instantiate(_bullet, _shotPoint.position, transform.rotation);
                _timeBtwShots = _startTimeBtwShots;
            }
        }
        else
        {
            _timeBtwShots -= Time.deltaTime;
        }

        if (_timeBtwKicks <= 0)
        {
            if (Input.GetMouseButtonDown(1))
            {
                Collider2D[] solids = Physics2D.OverlapCircleAll(_kickPos.position, _kickRange, _solid);
                for (int i = 0; i < solids.Length; i++)
                {
                    solids[i].GetComponent<Enemy>().TakeDamage(_kickDamage);
                }
                print("Ближний бой");
                _timeBtwKicks = _startTimeBtwKicks;
            }
        }
        else
        {
            _timeBtwKicks -= Time.deltaTime;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_kickPos.position, _kickRange);
    }
}
