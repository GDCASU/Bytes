using System;
using System.Collections;
using UnityEngine;

[System.Serializable]
public class Cooldown
{
    public float duration;
    float endTime;

    public void Start() => endTime = Time.time + duration;
    public bool IsOver { get => Time.time >= endTime; }

    public static IEnumerator Routine(WaitForSeconds durationWait, Action cooldownFinished)
    {
        yield return durationWait;
        cooldownFinished?.Invoke();
    }

    public static IEnumerator Routine(float duration, Action<float> cooldownProgressUpdated, Action cooldownFinished)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            cooldownProgressUpdated?.Invoke(elapsedTime);
            yield return null;
        }
        cooldownProgressUpdated(duration);
        cooldownFinished?.Invoke();
    }
}