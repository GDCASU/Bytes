/*
 * Author: Cristion Dominguez
 * Date: 13 Jan. 2023
 */

using System.Collections;
using UnityEngine;

public abstract class ReleaseAbility : MonoBehaviour, IWeaponAbility
{
    [SerializeField] protected int priority;
    protected bool isHolding;
    protected bool isReleasePatternRunning;
    protected Maneuver releaseManeuver;
    protected Coroutine releaseRoutine;

    public abstract WeaponAbilityType Type { get; }
    public WeaponAbilityPattern Pattern => WeaponAbilityPattern.Release;
    public abstract AmmoType ExpectedAmmo { get; }
    public Weapon WeaponHost { get; protected set; }
    public abstract string ReleaseName { get; }
    public abstract bool CanExecuteEvent { get; }
    public abstract float ChargeDuration { get; }
    public abstract float PersistDuration { get; }
    protected abstract void ExecuteEvent(float powerProportion);
    protected abstract void OnFailedEvent();

    protected virtual void Awake()
    {
        WeaponHost = GetComponent<Weapon>();
        releaseManeuver = new Maneuver(ReleaseName, priority, Release_Perform, Release_Pause, Release_Resume, Release_Halt);
    }

    public void Trigger(bool isStarting)
    {
        isHolding = isStarting;

        if (isStarting)
        {
            if (CanExecuteEvent)
                WeaponHost.EnqueueManeuver(releaseManeuver);
            else
                OnFailedEvent();
        }
    }

    public void Cancel() => releaseManeuver.Halt();

    protected virtual void Release_Perform(bool pauseImmediately)
    {
        if (!pauseImmediately)
            releaseRoutine = StartCoroutine(CR_RunReleasePattern());
    }

    protected virtual void Release_Pause()
    {
        if (isReleasePatternRunning)
        {
            if (releaseRoutine != null)
                StopCoroutine(releaseRoutine);
            isReleasePatternRunning = false;
        }
    }

    protected virtual void Release_Resume()
    {
        releaseRoutine = StartCoroutine(CR_RunReleasePattern());
    }

    protected virtual void Release_Halt()
    {
        if (isReleasePatternRunning)
        {
            if (releaseRoutine != null)
                StopCoroutine(releaseRoutine);
            isReleasePatternRunning = false;
            releaseManeuver.Dequeue();
        }
    }

    protected virtual IEnumerator CR_RunReleasePattern()
    {
        isReleasePatternRunning = true;

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
            {
                ExecuteEvent(elapsedTime / ChargeDuration);
                goto Finish;
            }
            yield return null;
        }

        elapsedTime = 0;
        while (elapsedTime < PersistDuration && isHolding)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        ExecuteEvent(1);

    Finish:
        Release_Halt();
    }
}
