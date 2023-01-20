/*
 * Author: Cristion Dominguez
 * Date: 13 Jan. 2023
 */

public enum WeaponAbilityType
{
    Primary = 0,
    Secondary = 1,
    Tertiary = 2,
    Reload = 3,
    Empty = 4
}

public enum WeaponAbilityPattern
{
    Tap,
    Repeat,
    Release,
    Continuity
}

public interface IWeaponAbility
{
    public WeaponAbilityType Type { get; }
    public WeaponAbilityPattern Pattern { get; }
    public AmmoType ExpectedAmmo { get; }
    public Weapon WeaponHost { get; }
    public void Trigger(bool isStarting);
    public void Cancel();
}