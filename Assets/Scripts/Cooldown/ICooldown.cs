using System;

public interface ICooldown
{
    public event Action<float> OnUpdate;
    public bool IsObservable { get; }
    public float Duration { get; set; }
    public bool IsActive { get; }
    public void Activate();
    public void Deactive();
}