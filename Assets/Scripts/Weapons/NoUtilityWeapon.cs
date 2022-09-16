using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoUtilityWeapon : Weapon2
{
    public override void PerformPrimaryAttack(bool isStarting)
    {
        if (primaryAttack)
            primaryAttack.Perform(isStarting);
    }

    public override void PerformSecondaryAttack(bool isStarting)
    {
        if (secondaryAttack)
            secondaryAttack.Perform(isStarting);
    }

    public override void PerformRestoration()
    {
        if (primaryAttack)
            primaryAttack.RestoreResource();
        if (secondaryAttack)
            secondaryAttack.RestoreResource();
    }

    public override void PerformUtility(bool isStarting) { }
}