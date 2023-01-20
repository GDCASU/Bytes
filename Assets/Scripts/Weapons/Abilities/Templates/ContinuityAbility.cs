/*
 * Author: Cristion Dominguez
 * Date: 13 Jan. 2023
 */

using System.Collections;
using UnityEngine;

public abstract class ContinuityAbility : MonoBehaviour, IWeaponAbility
{
    [SerializeField] protected int priority;
    protected bool isHolding;
    protected bool isContinuityPatternRunning;
    protected bool isEventExecuting;
    protected Maneuver continuityManeuver;
    protected Coroutine continuityRoutine;

    public abstract WeaponAbilityType Type { get; }
    public WeaponAbilityPattern Pattern => WeaponAbilityPattern.Continuity;
    public abstract AmmoType ExpectedAmmo { get; }
    public Weapon WeaponHost { get; protected set; }
    protected abstract string ContinuityName { get; }
    protected abstract bool CanExecuteEvent { get; }
    protected abstract float ChargeDuration { get; }
    protected abstract void StartEvent();
    protected abstract void StopEvent();
    protected abstract void OnFailedEvent();

    protected virtual void Awake()
    {
        WeaponHost = GetComponent<Weapon>();
        continuityManeuver = new Maneuver(ContinuityName, priority, Continuity_Perform, Continuity_Pause, Continuity_Resume, Continuity_Halt);
    }

    public void Trigger(bool isStarting)
    {
        isHolding = isStarting;

        if (isStarting)
        {
            if (CanExecuteEvent)
                WeaponHost.EnqueueManeuver(continuityManeuver);
            else
                OnFailedEvent();
        }
    }

    public void Cancel() => Continuity_Halt();

    protected virtual void Continuity_Perform(bool pauseImmediately)
    {
        if (!pauseImmediately)
            continuityRoutine = StartCoroutine(CR_RunContinuityPattern());
    }

    protected virtual void Continuity_Pause()
    {
        if (isContinuityPatternRunning)
        {
            if (isEventExecuting)
            {
                StopEvent();
                isEventExecuting = false;
            }

            if (continuityRoutine != null)
                StopCoroutine(continuityRoutine);
            isContinuityPatternRunning = false;
        }
    }

    protected virtual void Continuity_Resume()
    {
        continuityRoutine = StartCoroutine(CR_RunContinuityPattern());
    }

    protected virtual void Continuity_Halt()
    {
        if (isContinuityPatternRunning)
        {
            if (isEventExecuting)
            {
                StopEvent();
                isEventExecuting = false;
            }

            if (continuityRoutine != null)
                StopCoroutine(continuityRoutine);
            isContinuityPatternRunning = false;
            continuityManeuver.Dequeue();
        }
    }

    protected virtual IEnumerator CR_RunContinuityPattern()
    {
        isContinuityPatternRunning = true;

        if (!CanExecuteEvent)
        {
            OnFailedEvent();
            goto Finish;
        }

        float elapsedTime = 0;
        while (elapsedTime < ChargeDuration)
        {
            elapsedTime += Time.deltaTime;
            if (!isHolding)
                goto Finish;
            yield return null;
        }

        isEventExecuting = true;
        StartEvent();

        while (isHolding)
        {
            if (!CanExecuteEvent)
            {
                OnFailedEvent();
                break;
            }
            yield return null;
        }

        StopEvent();
        isEventExecuting = false;

    Finish:
        Continuity_Halt();
    }
}
