using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IWeaponSkill))]
public class WeaponExp : MonoBehaviour
{
    [SerializeField] AnimatorOverrideController _overrideController;

    public WeaponHandler Handler { get; private set; }

    IWeaponSkill _primarySkill, _secondarySkill, _tertiarySkill, _reloadSkill;
    IWeaponSkill _obtructingSkill;

    private void Awake()
    {
        IWeaponSkill[] skills = GetComponents<IWeaponSkill>();
        foreach (IWeaponSkill skill in skills)
        {
            switch (skill.Type)
            {
                case SkillType.Primary:
                    _primarySkill = skill;
                    break;
                case SkillType.Secondary:
                    _secondarySkill = skill;
                    break;
                case SkillType.Tertiary:
                    _tertiarySkill = skill;
                    break;
                case SkillType.Reload:
                    _reloadSkill = skill;
                    break;
                default:
                    continue;
            }
        }

        _primarySkill = _primarySkill ?? new EmptySkill();
        _secondarySkill = _secondarySkill ?? new EmptySkill();
        _tertiarySkill = _tertiarySkill ?? new EmptySkill();
        _reloadSkill = _reloadSkill ?? new EmptySkill();
    }

    private void OnEnable()
    {
        _primarySkill.ObstructionRelinquished += ResetObstructingSkill;
        _secondarySkill.ObstructionRelinquished += ResetObstructingSkill;
        _tertiarySkill.ObstructionRelinquished += ResetObstructingSkill;
        _reloadSkill.ObstructionRelinquished += ResetObstructingSkill;
    }

    private void OnDisable()
    {
        _primarySkill.ObstructionRelinquished -= ResetObstructingSkill;
        _secondarySkill.ObstructionRelinquished -= ResetObstructingSkill;
        _tertiarySkill.ObstructionRelinquished -= ResetObstructingSkill;
        _reloadSkill.ObstructionRelinquished -= ResetObstructingSkill;
    }

    public void PerformPrimaryAttack(bool isStarting)
    {
        if (!CanSkillReceiveInput(_primarySkill))
            return;

        _primarySkill.Perform(isStarting);
    }

    public void PerformSecondaryAttack(bool isStarting)
    {
        if (!CanSkillReceiveInput(_secondarySkill))
            return;

        _secondarySkill.Perform(isStarting);
    }

    public void PerformTertiaryAttack(bool isStarting)
    {
        if (!CanSkillReceiveInput(_tertiarySkill))
            return;

        _tertiarySkill.Perform(isStarting);
    }

    public void PerformReload()
    {
        if (!CanSkillReceiveInput(_reloadSkill))
            return;

        _reloadSkill.Perform(true);
    }

    public void Ready(WeaponHandler handler)
    {
        Handler = handler;
    }

    public void Store()
    {
        _primarySkill.Interrupt();
        _secondarySkill.Interrupt();
        _tertiarySkill.Interrupt();
        _reloadSkill.Interrupt();
    }

    bool CanSkillReceiveInput(IWeaponSkill skill)
    {
        if (_obtructingSkill == null && _obtructingSkill != _primarySkill)
        {
            if (_obtructingSkill.Priority > skill.Priority)
            {
                _obtructingSkill.Interrupt();
                _obtructingSkill = skill;
            }
            else
                return false;
        }

        return true;
    }

    void ResetObstructingSkill(IWeaponSkill skill)
    {
        if (_obtructingSkill == skill)
            _obtructingSkill = null;
    }
}
