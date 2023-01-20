/*
 * Author: Cristion Dominguez
 * Date: 13 Jan. 2023
 */

using System.Collections;
using UnityEngine;

public abstract class SequenceAbility : MonoBehaviour, IWeaponAbility
{
    [SerializeField] protected int priority;
    [SerializeField] protected bool shouldRepeat;
    protected bool isSequencePatternRunning;
    protected bool hasIntervalPassed = true;
    protected Maneuver sequenceManeuver;
    protected Coroutine sequenceRoutine;
    protected Coroutine intervalRoutine;

    public abstract WeaponAbilityType Type { get; }
    public WeaponAbilityPattern Pattern => shouldRepeat ? WeaponAbilityPattern.Repeat : WeaponAbilityPattern.Tap;
    public abstract AmmoType ExpectedAmmo { get; }
    public Weapon WeaponHost { get; protected set; }
    protected abstract string SequenceName { get; }
    protected abstract bool CanExecuteEvent { get; }
    protected abstract float ChargeDuration { get; }
    protected abstract float RepeatInterval { get; }
    protected abstract void ExecuteEvent();
    protected abstract void OnFailedEvent();

    protected virtual void Awake()
    {
        WeaponHost = GetComponent<Weapon>();
        sequenceManeuver = new Maneuver(SequenceName, priority, Sequence_Perform, Sequence_Pause, Sequence_Resume, Sequence_Halt);
    }

    public virtual void Trigger(bool isStarting)
    {
        if (isStarting)
        {
            if (CanExecuteEvent)
                WeaponHost.EnqueueManeuver(sequenceManeuver);
            else
                OnFailedEvent();
        }
        else
            Sequence_Halt();
    }

    public virtual void Cancel() => Sequence_Halt();

    protected virtual void Sequence_Perform(bool pauseImmediately)
    {
        if (!pauseImmediately)
            sequenceRoutine = StartCoroutine(CR_RunSequencePattern());
    }

    protected virtual void Sequence_Pause()
    {
        if (isSequencePatternRunning)
        {
            if (sequenceRoutine != null)
                StopCoroutine(sequenceRoutine);
            isSequencePatternRunning = false;
        }
    }

    protected virtual void Sequence_Resume()
    {
        sequenceRoutine = StartCoroutine(CR_RunSequencePattern());
    }

    protected virtual void Sequence_Halt()
    {
        if (isSequencePatternRunning)
        {
            if (sequenceRoutine != null)
                StopCoroutine(sequenceRoutine);
            isSequencePatternRunning = false;
            sequenceManeuver.Dequeue();
        }
    }

    protected virtual IEnumerator CR_RunSequencePattern()
    {
        isSequencePatternRunning = true;

        while (!hasIntervalPassed)
            yield return null;

        if (!CanExecuteEvent)
        {
            OnFailedEvent();
            goto Finish;
        }

        float elapsedTime = 0;
        while (elapsedTime < ChargeDuration)
        {
            elapsedTime += Time.deltaTime;
            print(elapsedTime);
            yield return null;
        }

        ExecuteEvent();
        intervalRoutine = StartCoroutine(CR_TrackInterval());

        if (!shouldRepeat)
            goto Finish;

        while (CanExecuteEvent)
        {
            if (hasIntervalPassed)
            {
                ExecuteEvent();
                intervalRoutine = StartCoroutine(CR_TrackInterval());
            }

            yield return null;
        }
        OnFailedEvent();

    Finish:
        Sequence_Halt();
    }

    protected IEnumerator CR_TrackInterval()
    {
        hasIntervalPassed = false;

        float elapsedTime = 0;
        while (elapsedTime < RepeatInterval)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        hasIntervalPassed = true;
    }
}
