using System;
using System.Collections;
using UnityEngine;

[System.Serializable]
public class FrameUpdaterCooldown : ICooldown
{
    public event Action<float> OnUpdate;
    public Action cooldownFinished;

    bool isActive;
    MonoBehaviour behaviour;

    public FrameUpdaterCooldown(MonoBehaviour behaviour) => this.behaviour = behaviour;

    public FrameUpdaterCooldown(MonoBehaviour behaviour, float duration)
    {
        this.behaviour = behaviour;
        Duration = duration;
    }

    public bool IsObservable => true;

    [field: SerializeField] public float Duration { get; set; }

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
        float elapsedTime = 0f;
        while (elapsedTime < Duration)
        {
            elapsedTime += Time.deltaTime;
            OnUpdate?.Invoke(elapsedTime);
            yield return null;
        }
        OnUpdate?.Invoke(Duration);
        cooldownFinished?.Invoke();
        isActive = false;
    }
}