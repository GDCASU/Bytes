using System;

public class EmptySkill : IWeaponSkill
{
#pragma warning disable CS0067
    public event Action<IWeaponSkill> Deactivated;
    public event Action ResourceExpended;
    public SkillStatus Status => SkillStatus.Inactive;
    public SkillType Type => SkillType.Empty;
    public int Priority => int.MaxValue;
    public AmmoType AmmoType => 0;
    public bool Obstructs(bool isStarting) => false;
    public bool CanPerform(bool isStarting) => true;
    public void Perform(bool isStarting, bool pauseImmediately = false) { }
    public void Pause() { }
    public void Resume() { }
    public void Halt() { }
}