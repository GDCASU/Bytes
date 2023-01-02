/*
 * Author: Cristion Dominguez
 * Date: 10 Oct. 2022
 */

using System;

public interface IWeaponWielder: ICombatant
{
    public event Action PrimaryAttackPerformed;
    public event Action PrimaryAttackCanceled;
    public event Action SecondaryAttackPerformed;
    public event Action SecondaryAttackCanceled;
    public event Action TertiaryAttackPerformed;
    public event Action TertiaryAttackCanceled;
    public event Action ReloadPerformed;
    public event Action ReloadCanceled;
    public event Action<int> SwitchWeaponPerformed;
}