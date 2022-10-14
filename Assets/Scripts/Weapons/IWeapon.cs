using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon2: MonoBehaviour
{
    [SerializeField] protected Attack primaryAttack, secondaryAttack;
    public WeaponHandler Controller { get; set; }

    public abstract void PerformPrimaryAttack(bool isStarting);
    public abstract void PerformSecondaryAttack(bool isStarting);
    public abstract void PerformRestoration();
    public abstract void PerformUtility(bool isStarting);
}