using System;

public class EmptySkill : IWeaponSkill
{
    public event Action<IWeaponSkill> Deactivated;
    public SkillStatus Status => SkillStatus.Inactive;
    public SkillType Type => SkillType.Empty;
    public int Priority => int.MaxValue;
    public BaseResource Resource => null;
    public bool Obstructs(bool isStarting) => false;
    public void Perform(bool isStarting, bool pauseImmediately = false) { }
    public void Pause() { }
    public void Resume() { }
    public void Halt() { }
}