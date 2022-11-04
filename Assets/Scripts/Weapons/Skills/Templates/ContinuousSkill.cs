using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ContinuousSkill : MonoBehaviour, IWeaponSkill
{
    [SerializeField] protected float chargeDuration;
    [SerializeField] protected float holdDuration;

    protected bool isInputActive;
    protected bool isPatternRunning;
    protected Weapon weapon;

    public event Action<IWeaponSkill> Deactivated;
    public SkillStatus Status => SkillStatus.Inactive;
    public SkillType Type => SkillType.Empty;
    public int Priority => int.MaxValue;
    public BaseResource Resource => null;

    protected virtual void Awake()
    {
        weapon = GetComponent<Weapon>();
    }

    public bool Obstructs(bool isStarting) => false;

    public void Perform(bool isStarting, bool pauseImmediately = false)
    {
        isInputActive = isStarting;
        if (isStarting && !isPatternRunning && CanPerform)
            StartCoroutine(CR_RunPattern());
    }

    public void Pause() { }

    public void Resume() { }

    public void Halt() { }

    protected IEnumerator CR_RunPattern()
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
            ExecuteManuever(elapsedTime / chargeDuration);
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

        ExecuteManuever(1f);
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
    protected abstract void ExecuteManuever(float powerProportion);
}
