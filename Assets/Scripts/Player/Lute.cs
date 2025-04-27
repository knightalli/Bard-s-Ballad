// Lute.cs
using UnityEngine;

public class Lute : MonoBehaviour
{
    [SerializeField] private float _offset;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _shotPoint;
    [SerializeField] private float _baseTimeBtwShots;
    [SerializeField] private Transform _kickPos;
    [SerializeField] private float _kickRange;
    [SerializeField] private float _startTimeBtwKicks;
    [SerializeField] private LayerMask _whatIsSolid;
    [SerializeField] private int _kickBaseDamage;
    [SerializeField] private PlayerStats _playerStats;

    private float _timeBtwShots;
    private float _timeBtwKicks;

    void Update()
    {
        if (Time.timeScale == 0f) return;

        Vector3 diff = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float rotZ = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ + _offset);

        if (_timeBtwShots <= 0f)
        {
            if (Input.GetMouseButton(0))
            {
                var bulletObj = Instantiate(_bulletPrefab, _shotPoint.position, transform.rotation);
                var bullet = bulletObj.GetComponent<Bullet>();
                bullet.Setup(_playerStats.currentPower);
                _timeBtwShots = _baseTimeBtwShots / Mathf.Max(1, _playerStats.currentSpeed);
            }
        }
        else _timeBtwShots -= Time.unscaledDeltaTime;

        if (_timeBtwKicks <= 0f)
        {
            if (Input.GetMouseButtonDown(1))
            {
                int totalKickDmg = _kickBaseDamage + _playerStats.currentPower;
                Collider2D[] hits = Physics2D.OverlapCircleAll(_kickPos.position, _kickRange, _whatIsSolid);
                foreach (var col in hits)
                    if (col.TryGetComponent<Enemy>(out var e))
                        e.TakeDamage(totalKickDmg);
                _timeBtwKicks = _startTimeBtwKicks;
            }
        }
        else _timeBtwKicks -= Time.unscaledDeltaTime;
    }
}
