/*
 * Author: Cristion Dominguez
 * Date: 13 Jan. 2023
 */

using System;
using UnityEngine;
using UnityEngine.Pool;

public class SimplePool<T> where T : PoolItem
{
    ObjectPool<T> _pool;
    Action _poolDisposed;

    public SimplePool(T originalItem, int capacity)
    {
        if (originalItem == null)
        {
            Debug.LogError("A pool item was not passed.");
            return;
        }

        _pool = new ObjectPool<T>
        (
            () =>
            {
                T item = UnityEngine.Object.Instantiate(originalItem);
                item.ReturningToPool = () => _pool.Release(item);
                _poolDisposed += item.OnPoolDisposed;
                return item;
            },
            (item) => item.OnGet(),
            (item) => { },
            (item) =>
            {
                item.ReturningToPool = null;
                item.OnPoolDisposed();
                _poolDisposed -= item.OnPoolDisposed;
            },
            false,
            capacity,
            capacity
        );
    }

    public T Get() => _pool.Get();

    public void Dispose()
    {
        _pool.Dispose();
        _poolDisposed?.Invoke();
    }
}