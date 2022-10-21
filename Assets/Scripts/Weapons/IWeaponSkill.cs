using System;

public enum SkillType
{
    Primary,
    Secondary,
    Tertiary,
    Reload,
    Empty
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
    public event Action<IWeaponSkill> ObstructionRelinquished;
    public bool IsObstructing { get; }
    public SkillType Type { get; }
    public int Priority { get; }
    public BaseResource Resource { get; }
    public void Perform(bool isStarting);
    public void Interrupt();
}
