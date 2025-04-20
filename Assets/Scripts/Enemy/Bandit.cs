using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bandit : Enemy
{
    [SerializeField] private int _customHealth;
    [SerializeField] private float _startTimeBtwAttack;
    [SerializeField] private Transform _playerTransform;

    [SerializeField] private Transform _enemyAttackPoint;
    [SerializeField] private int _customDamage;
    [SerializeField] private LayerMask _whatIsPlayer;
    [SerializeField] private GameObject _enemyBulletPrefab;

    private float _timeBtwAttack;
    private void Start()
    {
        SetHealth(_customHealth);
        SetDamage(_customDamage);
        _timeBtwAttack = _startTimeBtwAttack;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (IsStunned())
            return;

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

    private void RangedAttack()
    {
        Vector2 dir = (_playerTransform.position - _enemyAttackPoint.position).normalized;
               
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion rot = Quaternion.Euler(0f, 0f, angle);

        Instantiate(_enemyBulletPrefab, _enemyAttackPoint.position, rot);
    }
}
