using System;
using UnityEngine;
using UnityEngine.Pool;

public class SimplePool
{
    ObjectPool<IPoolItem> _pool;
    Action _poolDisposed;

    public SimplePool(GameObject itemPrefab, int capacity)
    {
        if (itemPrefab == null || itemPrefab.TryGetComponent<IPoolItem>(out _))
        {
            Debug.LogError(itemPrefab.name + " does not possess an IPoolItem script.");
            return;
        }

        _pool = new ObjectPool<IPoolItem>
        (
            () =>
            {
                GameObject obj = UnityEngine.Object.Instantiate(itemPrefab);
                IPoolItem item = obj.GetComponent<IPoolItem>();
                item.Finished += _pool.Release;
                _poolDisposed += item.OnPoolDisposed;
                return item;
            },
            (item) => item.OnGet(),
            (item) => { },
            (item) =>
            {
                item.Finished -= _pool.Release;
                item.OnPoolDisposed();
                _poolDisposed -= item.OnPoolDisposed;
            },
            false,
            capacity,
            capacity
        );
    }

    public IPoolItem Get() => _pool.Get();

    public void Dispose()
    {
        _pool.Dispose();
        _poolDisposed?.Invoke();
    }
}
