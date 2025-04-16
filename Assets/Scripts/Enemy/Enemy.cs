using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected int _health;       

    public void SetHealth(int value)
    {
        _health = value;
    }

    public int GetHealth() => _health;

    public void TakeDamage(int damage)
    {
        print("Нанесен урон " + damage + " здоровье " + _health);
        _health -= damage;
    }

    public void Dead()
    {
        if (_health <= 0) Destroy(gameObject); 
    }
}
