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
    [SerializeField] private AudioSource[] _audioSources;
    [SerializeField] private LuteSoundController _soundController;
    private float _timeBtwShots;
    private float _timeBtwKicks;
    private InventoryController _inventoryController;
    private float _lastSoundTime;

    void Start()
    {
        _inventoryController = FindObjectOfType<InventoryController>();
        if (_soundController == null)
            _soundController = GetComponent<LuteSoundController>();

        // Проверка, что все три источника назначены
        if (_audioSources == null || _audioSources.Length < 3)
            Debug.LogWarning("Назначьте ровно 3 AudioSource в массив _audioSources!", this);
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

                // --- Звуки ---
                if (_soundController != null)
                {
                    AudioClip[] clipsToPlay = _soundController.GetNextSounds();
                    if (clipsToPlay != null && _audioSources != null && _audioSources.Length >= 1)
                    {
                        for (int i = 0; i < _audioSources.Length && i < clipsToPlay.Length; i++)
                        {
                            var src = _audioSources[i];
                            if (src != null && clipsToPlay[i] != null && Time.time - _lastSoundTime >= _baseTimeBtwShots)
                            {
                                src.PlayOneShot(clipsToPlay[i]);
                            }
                        }
                        _lastSoundTime = Time.time;
                    }
                }

                _timeBtwShots = _baseTimeBtwShots;
            }
        }
        else _timeBtwShots -= Time.unscaledDeltaTime;

        if (_timeBtwKicks <= 0f)
        {
            if (Input.GetMouseButtonDown(1))
            {
                int totalKickDmg = _kickBaseDamage;
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
