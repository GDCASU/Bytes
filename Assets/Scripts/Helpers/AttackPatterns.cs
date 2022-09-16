using System;
using System.Collections;
using UnityEngine;

public static class Cooldown2
{
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

public interface ICooldown
{
    public IEnumerator Routine();
}

public class SilentCooldown: ICooldown
{
    public WaitForSeconds durationWait;
    public event Action OnFinish;

    public IEnumerator Routine()
    {
        yield return durationWait;
        OnFinish?.Invoke();
    }
}

public class LoudCooldown: ICooldown
{
    public float duration;
    public event Action<float> OnUpdate;
    public event Action OnFinish;

    public IEnumerator Routine()
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            OnUpdate?.Invoke(elapsedTime);
            yield return null;
        }
        OnUpdate(duration);
        OnFinish?.Invoke();
    }
}