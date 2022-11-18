using System;

public interface IResource
{
    public event Action<int> OnUpdate;
    public int Max { get; }
    public int Current { get; }
    public void Drain(int amount);
    public void Fill(int amount);
}