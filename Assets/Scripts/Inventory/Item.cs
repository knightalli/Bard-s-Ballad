// Item.cs
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Item : MonoBehaviour
{
    public ItemDatabase database;
    public float pickupRange = 2f;
    public Inventory _inventory;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private static List<ItemSO> _available;
    private ItemSO _itemData;    
    private Transform _player;

    void Awake()
    {
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
        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) &&
            Vector2.Distance(transform.position, _player.position) <= pickupRange)
        {
            _inventory.AddItem(_itemData);
            Destroy(gameObject);
        }
    }
}
