using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ContinuousAttack : Attack  // MODIFY
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
