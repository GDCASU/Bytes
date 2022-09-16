using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IResource
{
    public int Max { get; }
    public int Current { get; }
    public event Action<int> OnUpdate;
    public void Drain(int amount);
    public void Restore(int amount);
}

public abstract class AbstractResource: MonoBehaviour
{
    public event Action<int> OnUpdate;
    public abstract int Max { get; }
    public abstract int Current { get; }
    public abstract void Drain(int amount);
    public abstract void Restore(int amount);
}