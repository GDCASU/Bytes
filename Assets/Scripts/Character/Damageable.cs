/*
 * Author: Cristion Dominguez
 * Date: 19 Jan. 2023
 */

using System;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    public event Action<int> DamageReceived;
    public event Action<int> HealingReceived;
    public event Action Died;

    [SerializeField] StaticResource _health;
    [SerializeField] CharacterAllegiance _allegiance;
    [SerializeField] bool _isVisible = true;
    [SerializeField] Transform[] _detectionPoints;
    [SerializeField] IntEventChannelSO _healthUpdateEvent = default;
    [SerializeField] VoidEventChannelSO _deathEvent = default;

    public int CurrentHealth => _health.Current;
    public CharacterAllegiance Allegiance => _allegiance;
    public bool IsDead => _health.Current <= 0;
    public bool IsVisible => _isVisible;
    public Transform[] DetectionPoints => _detectionPoints;
    public Hurtbox[] Hurtboxes { get; private set; }

    void Awake()
    {
        Hurtboxes = GetComponentsInChildren<Hurtbox>();
    }

    public void ReceiveDamage(int damage)
    {
        if (IsDead)
            return;

        _health.Drain(damage);

        if (_health.Current <= 0)
        {
            Died?.Invoke();
            _deathEvent?.RaiseEvent();
        }

        DamageReceived?.Invoke(damage);
        _healthUpdateEvent?.RaiseEvent(_health.Current);
    }

    public void ReceiveHealing(int healing)
    {
        _health.Fill(healing);

        HealingReceived?.Invoke(healing);
        _healthUpdateEvent?.RaiseEvent(_health.Current);
    }
}
