using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    [SerializeField] int _maxHealth;
    [SerializeField] int _health;
    [SerializeField] IntEventChannelSO _healthUpdateEvent = default;
    [SerializeField] VoidEventChannelSO _deathEvent = default;

    public int MaxHealth => _maxHealth;
    public int Health => _health;
    public bool IsDead => _health <= 0;

    private void OnEnable()
    {
        _health = _maxHealth;
    }

    public void ReceiveDamage(int damage)
    {
        if (IsDead)
            return;

        _health -= damage;

        if (_health <= 0)
        {
            _health = 0;
            _deathEvent.RaiseEvent();
        }

        _healthUpdateEvent.RaiseEvent(_health);
    }

    public void ReceiveHealth(int health)
    {
        _health += health;

        if (_health > _maxHealth)
            _health = _maxHealth;

        _healthUpdateEvent.RaiseEvent(_health);
    }
}
