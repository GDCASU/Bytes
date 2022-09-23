using System;
using UnityEngine;

[System.Serializable]
public class SilentCooldown : ICooldown
{
    public event Action<float> OnUpdate { add { } remove { } }

    public SilentCooldown()
    {
        Duration = 0f;
    }

    public SilentCooldown(float duration)
    {
        Duration = duration;
    }

    public bool IsObservable => false;

    [field: SerializeField] public float Duration { get; set; }

    public bool IsActive => Time.time < EndTime;

    public float EndTime { get; private set; }

    public void Activate() => EndTime = Time.time + Duration;

    public void Deactive() => EndTime = Time.time;
}
