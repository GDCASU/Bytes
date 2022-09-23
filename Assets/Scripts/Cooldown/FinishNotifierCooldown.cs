using System;
using System.Collections;
using UnityEngine;

[System.Serializable]
public class FinishNotifierCooldown : ICooldown
{
    public event Action<float> OnUpdate { add { } remove { } }
    public Action cooldownFinished;
    
    WaitForSeconds durationWait = new WaitForSeconds(0f);
    bool isActive;
    MonoBehaviour behaviour;

    public FinishNotifierCooldown(MonoBehaviour behaviour) => this.behaviour = behaviour;

    public FinishNotifierCooldown(MonoBehaviour behaviour, float duration)
    {
        this.behaviour = behaviour;
        Duration = duration;
    }

    public bool IsObservable => false;

    [field: SerializeField, OnChangedCall(nameof(InstantiateDurationWait))] public float Duration { get; set; }

    public bool IsActive => isActive;

    public void Activate() => behaviour.StartCoroutine(Routine());

    public void Deactive()
    {
        behaviour.StopCoroutine(Routine());
        isActive = false;
    }

    public IEnumerator Routine()
    {
        isActive = true;
        yield return durationWait;
        cooldownFinished?.Invoke();
        isActive = false;
    }

    public void InstantiateDurationWait() => durationWait = new WaitForSeconds(Duration);
}