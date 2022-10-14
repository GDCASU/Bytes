using System;
using UnityEngine;

public abstract class Attack : MonoBehaviour
{
    public virtual bool HasResource { get => Resource; }
    public abstract BaseResource Resource { get; }
    public virtual bool HasCooldown { get => Cooldown != null; }
    public abstract ICooldown Cooldown { get; }
    public abstract bool CanAttemptRestore { get; }
    public abstract bool IsActive { get; }
    public abstract void Perform(bool isStarting);
    public abstract void RestoreResource();
    public abstract void Interrupt();

    /*
    public abstract bool AddResourceObserver(Action<int> resourceUpdated);
    public abstract void RemoveResourceObserver(Action<int> resourceUpdated);
    public abstract bool AddCooldownObserver(Action<float> cooldownUpdated);
    public abstract void RemoveCooldownObserver(Action<float> cooldownUpdated);
    */
}