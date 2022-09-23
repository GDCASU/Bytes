using System;
using UnityEngine;

public abstract class BaseResource: MonoBehaviour
{
    [field: SerializeField] public BaseResource parent { get; protected set; }

    public abstract event Action<int> OnUpdate;
    public abstract int Max { get; }
    public abstract int Current { get; }
    public abstract bool IsObservable { get; }
    public abstract void Drain(int amount);
    public abstract void Restore(int amount);
}