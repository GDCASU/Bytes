using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoUtilityWeapon : Weapon2
{
    [SerializeField] float reloadDuration;

    bool isRestoring;

    public override void PerformPrimaryAttack(bool isStarting)
    {
        if (!isRestoring && primaryAttack)
            primaryAttack.Perform(isStarting);
    }

    public override void PerformSecondaryAttack(bool isStarting)
    {
        if (!isRestoring && secondaryAttack)
            secondaryAttack.Perform(isStarting);
    }

    public override void PerformRestoration()
    {
        if (primaryAttack && secondaryAttack && primaryAttack.CanAttemptRestore && secondaryAttack.CanAttemptRestore)
        {
            primaryAttack.RestoreResource();
            secondaryAttack.RestoreResource();
        }
        else if (primaryAttack && primaryAttack.CanAttemptRestore)
        {
            primaryAttack.RestoreResource();
        }
        else if (secondaryAttack && secondaryAttack.CanAttemptRestore)
        {
            secondaryAttack.RestoreResource();
        }
    }

    public override void PerformUtility(bool isStarting) { }
}