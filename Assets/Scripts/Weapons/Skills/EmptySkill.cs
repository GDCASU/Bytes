using System;

public class EmptySkill : IWeaponSkill
{
    public event Action<IWeaponSkill> ObstructionRelinquished;
    public bool IsObstructing => false;
    public SkillType Type => SkillType.Empty;
    public int Priority => int.MaxValue;
    public BaseResource Resource => null;
    public void Perform(bool isStarting) { }
    public void Interrupt() { }
}