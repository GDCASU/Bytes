using System;

public enum SkillStatus
{
    Inactive,
    Active,
    Paused
}

public interface IWeaponSkill
{
    public event Action<IWeaponSkill> Deactivated;
    public event Action ResourceExpended;
    public SkillStatus Status { get; }
    public WeaponAbilityType Type { get; }
    public int Priority { get; }
    public AmmoType AmmoType { get; }
    public bool Obstructs(bool isStarting);
    public bool CanPerform(bool isStarting);
    public void Perform(bool isStarting, bool pauseImmediately = false);
    public void Pause();
    public void Resume();
    public void Halt();
}
