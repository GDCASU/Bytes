using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SingleOrRepeatingSkill : MonoBehaviour, IWeaponSkill
{
    [SerializeField] protected SkillType type;
    [SerializeField] protected int priority;
    [SerializeField] protected BaseResource resource;
    [SerializeField] protected float delayDuration;
    [SerializeField] protected bool shouldRepeat;

    protected SkillStatus status = SkillStatus.Inactive;
    protected bool isInputActive;
    protected bool isPatternRunning;
    protected Weapon weapon;

    public event Action<IWeaponSkill> Deactivated;

    public SkillStatus Status => status;
    public SkillType Type => type;
    public int Priority => priority;
    public BaseResource Resource => resource;

    public abstract float RepeatInterval { get; }
    protected abstract bool CanPerform { get; }
    protected abstract void UpdateDelayProgress(float elapsedTime);
    protected abstract void UpdateIntervalProgress(float elapsedTime);
    protected abstract void ExecuteManuever();

    protected virtual void Awake()
    {
        weapon = GetComponent<Weapon>();
    }

    public bool Obstructs(bool isStarting) => isStarting;

    public void Perform(bool isStarting, bool pauseImmediately = false)
    {
        isInputActive = isStarting;

        if (isStarting && !isPatternRunning && CanPerform)
        {
            if (shouldRepeat)
            {
                ExecuteManuever();
            }
            else
            {
                StartCoroutine(CR_RunPattern());
            }
        }
    }

    public void Pause()
    {
        status = SkillStatus.Paused;
    }

    public void Resume() { }

    public void Halt() { }

    protected IEnumerator CR_RunPattern()
    {
        isPatternRunning = true;
        status = SkillStatus.Active;

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

        ExecuteManuever();
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
                    ExecuteManuever();
                    elapsedTime = 0f;
                    goto CheckIfCanPerform;
                }

                UpdateIntervalProgress(elapsedTime);
                yield return null;
            }
        }

        isPatternRunning = false;
        status = SkillStatus.Inactive;
    }

    protected void StopPattern()
    {
        StopCoroutine(CR_RunPattern());
        isPatternRunning = false;
    }
}
