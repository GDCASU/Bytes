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
        if (primaryAttack && secondaryAttack && primaryAttack.CanRestoreNow && secondaryAttack.CanRestoreNow)
        {
            primaryAttack.RestoreResource();
            secondaryAttack.RestoreResource();
        }
        else if (primaryAttack && primaryAttack.CanRestoreNow)
        {
            primaryAttack.RestoreResource();
        }
        else if (secondaryAttack && secondaryAttack.CanRestoreNow)
        {
            secondaryAttack.RestoreResource();
        }
    }

    public override void PerformUtility(bool isStarting) { }
}