using System;

public interface IResource
{
    public event Action<int> Updated;
    public int Max { get; set;  }
    public int Current { get; }
    public int Drain(int amount);
    public void Fill(int amount);
}