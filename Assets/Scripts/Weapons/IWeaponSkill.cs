using System;

public enum SkillType
{
    Primary = 0,
    Secondary = 1,
    Tertiary = 2,
    Reload = 3,
    Empty = 4
}

public enum SkillStatus
{
    Inactive,
    Active,
    Paused
}

public enum SkillPattern
{
    Tap,
    Repeat,
    HoldAndRelease,
    Continuous
}

public interface IWeaponSkill
{
    public event Action<IWeaponSkill> Deactivated;
    public SkillStatus Status { get; }
    public SkillType Type { get; }
    public int Priority { get; }
    public BaseResource Resource { get; }
    public bool Obstructs(bool isStarting);
    public void Perform(bool isStarting, bool pauseImmediately = false);
    public void Pause();
    public void Resume();
    public void Halt();
}
