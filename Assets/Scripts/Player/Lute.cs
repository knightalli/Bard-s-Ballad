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
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip[] _shotClips;
    private int _currentShotIndex = 0;
    private float _timeBtwShots;
    private float _timeBtwKicks;
    private InventoryController _inventoryController;


    void Start()
    {
        _inventoryController = FindObjectOfType<InventoryController>();

        if (_shotClips == null || _shotClips.Length == 0)
            Debug.LogWarning("Не назначены звуковые клипы выстрелов на Lute", this);
    }


    void Update()
    {
        if (Time.timeScale == 0f) return;
        if (_inventoryController != null && _inventoryController.IsInventoryOpen) return;

        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 direction = Input.mousePosition - screenPos;
        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ + _offset);

        if (_timeBtwShots <= 0f)
        {
            if (Input.GetMouseButton(0))
            {
                var bulletObj = Instantiate(
                    _bulletPrefab,
                    _shotPoint.position,
                    _shotPoint.rotation);
                var bullet = bulletObj.GetComponent<Bullet>();
                bullet.Setup(_playerStats.currentPower);

                if (_audioSource != null && _shotClips != null && _shotClips.Length > 0)
                {
                    int clipIndex = _currentShotIndex % _shotClips.Length;
                    _audioSource.PlayOneShot(_shotClips[clipIndex]);
                    _currentShotIndex = (clipIndex + 1) % _shotClips.Length;
                }

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
