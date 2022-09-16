using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attack: MonoBehaviour
{
    [SerializeField] protected AbstractResource resource;

    public abstract void Perform(bool isStarting);
    public abstract void RestoreResource();
    public abstract bool AddResourceObserver(Action<int> resourceUpdated);
    public abstract void RemoveResourceObserver(Action<int> resourceUpdated);
    public abstract bool AddCooldownObserver(Action<float> cooldownUpdated);
    public abstract void RemoveCooldownObserver(Action<float> cooldownUpdated);
}

public abstract class SingleOrRepeatingAttack : Attack
{
    [SerializeField] protected float delayDuration;
    [SerializeField] protected bool shouldRepeat;
    
    protected bool isInputActive;
    protected bool isRepeatingPatternRunning;

    protected abstract float RepeatInterval { get; }

    public override void Perform(bool isStarting)
    {
        isInputActive = isStarting;

        if (isStarting && CanPerform && !isRepeatingPatternRunning)
        {
            if (shouldRepeat)
            {
                PerformManuever();
            }
            else
            {
                StartCoroutine(CR_RunRepeatingPattern());
            }
        }            
    }

    public IEnumerator CR_RunRepeatingPattern()
    {
        isRepeatingPatternRunning = true;

        float elapsedTime = 0f;
        while (isInputActive && elapsedTime < delayDuration)
        {
            elapsedTime += Time.deltaTime;
            UpdateDelayProgress(elapsedTime);
            yield return null;
        }

        if (!isInputActive)
            yield break;
        else
            UpdateDelayProgress(elapsedTime);

        PerformManuever();
        elapsedTime = 0f;

        CheckIfCanPerform:
        if (CanPerform)
        {
            while (isInputActive)
            {
                if (elapsedTime < RepeatInterval)
                    elapsedTime += Time.deltaTime;
                else
                {
                    PerformManuever();
                    elapsedTime = 0f;
                    goto CheckIfCanPerform;
                }

                UpdateIntervalProgress(elapsedTime);
                yield return null;
            }
        }

        isRepeatingPatternRunning = false;
    }

    protected void StopRepeatingPattern()
    {
        StopCoroutine(CR_RunRepeatingPattern());
        isRepeatingPatternRunning = false;
    }

    protected abstract bool CanPerform { get; }
    protected abstract void UpdateDelayProgress(float elapsedTime);
    protected abstract void UpdateIntervalProgress(float elapsedTime);
    protected abstract void PerformManuever();

}

public abstract class HoldReleaseAttack: Attack
{
    [SerializeField] protected float chargeDuration;
    [SerializeField] protected float holdDuration;

    protected bool isInputActive;
    protected bool isPatternRunning;

    public override void Perform(bool isStarting)
    {
        isInputActive = isStarting;
        if (isStarting && CanPerform)
            StartCoroutine(CR_RunPattern());
    }

    public IEnumerator CR_RunPattern()
    {
        isPatternRunning = true;

        float elapsedTime = 0f;
        while (isInputActive && elapsedTime < chargeDuration)
        {
            elapsedTime += Time.deltaTime;
            UpdateChargeProgress(elapsedTime);
            yield return null;
        }

        if (!isInputActive)
        {
            PerformManuever(elapsedTime / chargeDuration);
            yield break;
        }
        else
            UpdateChargeProgress(chargeDuration);

        elapsedTime = 0f;
        while (isInputActive && elapsedTime < holdDuration)
        {
            elapsedTime += Time.deltaTime;
            UpdateHoldProgress(elapsedTime);
            yield return null;
        }

        PerformManuever(1f);
        isPatternRunning = false;
    }

    protected void StopPattern()
    {
        StopCoroutine(CR_RunPattern());
        isPatternRunning = false;
    }

    protected abstract bool CanPerform { get; }
    protected abstract void UpdateChargeProgress(float elapsedTime);
    protected abstract void UpdateHoldProgress(float elapsedTime);
    protected abstract void PerformManuever(float powerProportion);
}

