using System;
using UnityEngine;

[Serializable]
public class StaticResource : IResource
{
    public event Action<int> Updated;
    public event Action<int> Drained;
    public event Action<int> Filled;

    [SerializeField] int _max;
    [SerializeField] int _current;

    public int Max
    {
        get => _max;
        set
        {
            if (value < 1)
                _max = 0;
            else
                _max = value;
        }
    }

    public int Current => _current;

    public int Drain(int amount)
    {
        if (amount < 1)
            return 0;

        int drained;
        if (_current >= amount)
        {
            drained = amount;
            _current -= amount;
        }
        else
        {
            drained = _current;
            _current = 0;
        }

        Updated?.Invoke(_current);
        Drained?.Invoke(_current);

        return drained;
    }

    public void Fill(int amount)
    {
        _current += amount;

        if (_current > _max)
            _current = _max;

        Updated?.Invoke(_current);
        Filled?.Invoke(_current);
    }
}