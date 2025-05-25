// Item.cs
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Item : MonoBehaviour
{
    public ItemDatabase database;
    public float pickupRange = 2f;
    private Inventory _inventory;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private static List<ItemSO> _available;
    private ItemSO _itemData;    
    private Transform _player;

    void Awake()
    {
        if (_spriteRenderer == null)
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            if (_spriteRenderer == null)
            {
                _spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
            }
        }

        if (database == null)
        {
            Debug.LogError($"ItemDatabase не назначен для предмета {gameObject.name}!");
            Destroy(gameObject);
            return;
        }

        if (_available == null)
            _available = new List<ItemSO>(database.allItems);

        if (_available.Count == 0)
        {
            Debug.LogWarning("No more items left in database");
            Destroy(gameObject);
            return;
        }

        int idx = Random.Range(0, _available.Count);
        _itemData = _available[idx];
        _available.RemoveAt(idx);

        _spriteRenderer.sprite = _itemData.icon;
    }

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (_player == null)
        {
            Debug.LogError("Player не найден на сцене!");
            Destroy(gameObject);
            return;
        }

        _inventory = _player.GetComponent<Inventory>();
        if (_inventory == null)
        {
            _inventory = _player.gameObject.AddComponent<Inventory>();
            Debug.Log("Inventory компонент был автоматически добавлен на игрока");
        }
    }

    void Update()
    {
        if (_player == null || _inventory == null) return;

        if (Input.GetKeyDown(KeyCode.E) &&
            Vector2.Distance(transform.position, _player.position) <= pickupRange)
        {
            _inventory.AddItem(_itemData);
            Destroy(gameObject);
        }
    }
}
