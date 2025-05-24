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
    [SerializeField] private AudioClip[] _defaultShotClips;
    [SerializeField] private LuteSoundController _soundController;
    private float _timeBtwShots;
    private float _timeBtwKicks;
    private InventoryController _inventoryController;
    private AudioSource[] _additionalAudioSources;
    private float _lastSoundTime;

    void Start()
    {
        _inventoryController = FindObjectOfType<InventoryController>();
        if (_soundController == null)
            _soundController = GetComponent<LuteSoundController>();

        // Создаем дополнительные AudioSource для одновременного проигрывания звуков
        _additionalAudioSources = new AudioSource[2]; // Максимум 2 дополнительных источника (всего 3 с основным)
        for (int i = 0; i < _additionalAudioSources.Length; i++)
        {
            _additionalAudioSources[i] = gameObject.AddComponent<AudioSource>();
            _additionalAudioSources[i].playOnAwake = false;
        }

        if (_defaultShotClips == null || _defaultShotClips.Length == 0)
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

                if (_audioSource != null)
                {
                    AudioClip[] clipsToPlay = null;
                    
                    // Пробуем получить звуки из активных предметов
                    if (_soundController != null)
                    {
                        clipsToPlay = _soundController.GetNextSounds();
                    }
                    
                    // Если нет звуков из предметов, используем стандартный
                    if (clipsToPlay == null && _defaultShotClips != null && _defaultShotClips.Length > 0)
                    {
                        if (Time.time - _lastSoundTime >= _baseTimeBtwShots)
                        {
                            _audioSource.PlayOneShot(_defaultShotClips[Random.Range(0, _defaultShotClips.Length)]);
                            _lastSoundTime = Time.time;
                        }
                    }
                    else if (clipsToPlay != null)
                    {
                        // Проигрываем все полученные звуки
                        for (int i = 0; i < clipsToPlay.Length; i++)
                        {
                            if (clipsToPlay[i] != null)
                            {
                                if (i == 0)
                                {
                                    if (Time.time - _lastSoundTime >= _baseTimeBtwShots)
                                    {
                                        _audioSource.PlayOneShot(clipsToPlay[i]);
                                        _lastSoundTime = Time.time;
                                    }
                                }
                                else if (i - 1 < _additionalAudioSources.Length)
                                {
                                    _additionalAudioSources[i - 1].PlayOneShot(clipsToPlay[i]);
                                }
                            }
                        }
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
