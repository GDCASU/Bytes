using System;
using System.Collections;
using UnityEngine;

public abstract class SingleOrRepeatingSkill : MonoBehaviour, IWeaponSkill
{
    public event Action<IWeaponSkill> Deactivated;
#pragma warning disable CS0067
    public event Action ResourceExpended;

    [SerializeField] protected SkillType type;
    [SerializeField] protected int priority;
    [SerializeField] protected AmmoType ammoType;
    [SerializeField] protected float chargeDuration;
    [SerializeField] protected bool shouldRepeat;
    protected SkillStatus status = SkillStatus.Inactive;
    protected bool isPatternRunning;
    protected StaticResource ammo;

    public SkillStatus Status => status;
    public SkillType Type => type;
    public int Priority => priority;
    public AmmoType AmmoType => ammoType;
    public StaticResource Ammo
    {
        get => ammo;
        set => ammo = value;
    }

    public abstract float RepeatInterval { get; }
    protected abstract void UpdateChargeProgress(float elapsedTime);
    protected abstract void UpdateIntervalProgress(float elapsedTime);
    protected abstract void ExecuteManuever();
    public abstract bool CanPerform(bool isStarting);
    public virtual bool Obstructs(bool isStarting) => isStarting;

    public virtual void Perform(bool isStarting, bool pauseImmediately = false)
    {
        switch (status)
        {
            case SkillStatus.Inactive:
                if (isStarting)
                {
                    if (!pauseImmediately)
                    {
                        if (!isPatternRunning)
                        {
                            status = SkillStatus.Active;
                            StartCoroutine(CR_RunPattern());
                        }
                    }
                    else
                    {
                        status = SkillStatus.Paused;
                    }
                }
                break;
            
            default:
                if (!isStarting)
                {
                    status = SkillStatus.Inactive;
                    Deactivated?.Invoke(this);
                }
                break;
        }
    }

    public void Pause()
    {
        status = SkillStatus.Paused;
    }

    public void Resume()
    {
        if (CanPerform(true))
        {
            status = SkillStatus.Active;
            StartCoroutine(CR_RunPattern());
        }
        else
        {
            status = SkillStatus.Inactive;
            Deactivated?.Invoke(this);
        }
    }

    public void Halt()
    {
        status = SkillStatus.Inactive;
    }

    protected virtual IEnumerator CR_RunPattern()
    {
        if (isPatternRunning)
            yield break;

        isPatternRunning = true;

        float elapsedTime = 0f;
        while (status == SkillStatus.Active && elapsedTime < chargeDuration)
        {
            elapsedTime += Time.deltaTime;
            UpdateChargeProgress(elapsedTime);
            yield return null;
        }

        if (status != SkillStatus.Active)
            yield break;
        else
            UpdateChargeProgress(elapsedTime);

        ExecuteManuever();
        if (!shouldRepeat)
            yield break;

        elapsedTime = 0f;
        while (status == SkillStatus.Active && CanPerform(true))
        {
            if (elapsedTime < RepeatInterval)
                elapsedTime += Time.deltaTime;
            else
            {
                ExecuteManuever();
                elapsedTime = 0f;
            }

            UpdateIntervalProgress(elapsedTime);
            yield return null;
        }

        isPatternRunning = false;
    }
}
