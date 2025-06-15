using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bandit : Enemy
{
    [SerializeField] private int _customHealth;
    [SerializeField] private float _startTimeBtwAttack;
    [SerializeField] private Transform _enemyAttackPoint;
    [SerializeField] private int _customDamage;
    [SerializeField] private LayerMask _whatIsPlayer;
    [SerializeField] private GameObject _enemyBulletPrefab;
    public Animator animator;

    private float _timeBtwAttack;
    private Transform _playerTransform;

    protected override void Start()
    {
        base.Start();
        
        // Находим игрока по тегу
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            _playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("Игрок не найден на сцене! Убедитесь, что у игрока установлен тег 'Player'");
        }

        SetHealth(_customHealth);
        SetDamage(_customDamage);
        _timeBtwAttack = _startTimeBtwAttack;
    }

    protected override void Update()
    {
        base.Update();
        
        if (_playerTransform == null)
        {
            // Пытаемся найти игрока, если ссылка потеряна
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                _playerTransform = player.transform;
            }
            return;
        }

        if (!IsStunned())
        {
            LookAtPlayer();
        }

        if (_timeBtwAttack <= 0f)
        {
            RangedAttack();
            _timeBtwAttack = _startTimeBtwAttack;
        }
        else
        {
            _timeBtwAttack -= Time.deltaTime;
        }
    }

    private void LookAtPlayer()
    {
        if (_playerTransform == null) return;

        Vector3 direction = (_playerTransform.position - transform.position).normalized;
        sr.flipX = direction.x < 0;
    }

    private void RangedAttack()
    {
        if (_playerTransform == null) return;

        Vector2 dir = (_playerTransform.position - _enemyAttackPoint.position).normalized;
               
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion rot = Quaternion.Euler(0f, 0f, angle);

        Instantiate(_enemyBulletPrefab, _enemyAttackPoint.position, rot);
    }
}
