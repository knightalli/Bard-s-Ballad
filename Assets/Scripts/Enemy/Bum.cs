using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bum : Enemy
{
    [SerializeField] private int _customHealth;
    [SerializeField] private Transform _player;
    [SerializeField] private float _speed = 3f;

    void Start()
    {
        SetHealth(_customHealth);
    }

    void Update()
    {
        Dead();
        MoveTowardsPlayer();
    }

    private void MoveTowardsPlayer()
    {
        Vector3 direction = (_player.position - transform.position).normalized;
        transform.position += direction * _speed * Time.deltaTime;
    }
}
