using System;
using UnityEngine;

[Serializable]
public class StaticResource : IResource
{
    public event Action<int> OnUpdate;

    [SerializeField] int _max;
    [SerializeField] int _current;

    public int Max => _max;
    public int Current => _current;

    public void Drain(int amount)
    {
        _current -= amount;

        if (_current < 0)
            _current = 0;

        OnUpdate?.Invoke(_current);
    }

    public void Fill(int amount)
    {
        _current += amount;

        if (_current > _max)
            _current = _max;

        OnUpdate?.Invoke(_current);
    }
}