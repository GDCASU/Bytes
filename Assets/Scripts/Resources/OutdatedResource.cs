using System;
using System.Collections;
using UnityEngine;

public class OutdatedResource : MonoBehaviour
{
    public event Action<int> OnUpdate;

    [SerializeField] int _max;
    [SerializeField] int _current;
    [SerializeField] bool _isObservable;
    [SerializeField] bool _doesRegenerate;
    [SerializeField] float _regenDelay;
    [SerializeField] int _regenPerSecond;

    WaitForSeconds _delayWait;
    WaitForSeconds _regenWait;
    bool _isRegenerating;

    public int Max => _max;
    public int Current => _current;
    public bool IsObservable => _isObservable;

    private void Awake()
    {
        _current = _max;
    }

    private void OnValidate()
    {
        _delayWait = new WaitForSeconds(_regenDelay);
        _regenWait = new WaitForSeconds(1f / _regenPerSecond);
    }

    public void Drain(int amount)
    {
        _current -= amount;

        if (_current < 0)
            _current = 0;

        if (_isObservable)
            OnUpdate?.Invoke(_current);

        if (!_doesRegenerate)
            return;

        if (_isRegenerating)
            StopCoroutine(CR_Regenerate());
        StartCoroutine(CR_Regenerate());
    }

    public void Fill(int amount)
    {
        _current += amount;

        if (_current > _max)
            _current = _max;

        if (_isObservable)
            OnUpdate?.Invoke(_current);
    }

    public void ReplenishFrom(IResource resource)
    {
        if (resource == null || resource.Current <= 0)
            return;

        if (resource.Current >= _max)
            _current = _max;
        else
            _current = resource.Current;

        resource.Drain(_current);

        if (_isObservable)
            OnUpdate?.Invoke(_current);
    }

    private IEnumerator CR_Regenerate()
    {
        _isRegenerating = true;

        yield return _delayWait;

        while (_current < _max)
        {
            _current++;

            if (_isObservable)
                OnUpdate?.Invoke(_current);

            yield return _regenWait;
        }

        _current = _max;
        if (_isObservable)
            OnUpdate?.Invoke(_current);

        _isRegenerating = false;
    }
}
