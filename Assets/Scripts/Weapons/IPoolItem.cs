using System;
using UnityEngine;

public interface IPoolItem
{
    public event Action<IPoolItem> Finished;
    public GameObject gameObject { get; }
    public void OnGet();
    public void OnPoolDisposed();
}
