/*
 * Author: Cristion Dominguez
 * Date: 13 Jan. 2023
 */

using System.Collections;
using UnityEngine;

public class ReloadAbility : MonoBehaviour, IWeaponAbility
{
    [SerializeField] int _priority;
    [SerializeField] float _duration;
    Maneuver _maneuver;
    Coroutine _routine;
    WaitForSeconds _reloadWait;

    public WeaponAbilityType Type => WeaponAbilityType.Reload;
    public WeaponAbilityPattern Pattern => WeaponAbilityPattern.Tap;
    public AmmoType ExpectedAmmo => AmmoType.None;
    public Weapon WeaponHost { get; private set; }

    void Awake()
    {
        WeaponHost = GetComponent<Weapon>();
        _maneuver = new Maneuver("Reload", _priority, Perform, Pause, Resume, Halt);
    }

    void OnValidate() => _reloadWait = new WaitForSeconds(_duration);

    public void Trigger(bool isStarting)
    {
        if (isStarting)
        {
            bool isEmpty = true;
            foreach (AmmoType ammoType in WeaponHost.ExpectedAmmos)
            {
                StaticResource handlerAmmo = WeaponHost.Handler.Inventory.GetAmmo(ammoType);
                if (handlerAmmo.Current > 0)
                {
                    isEmpty = false;
                    break;
                }
            }

            if (!isEmpty)
                WeaponHost.EnqueueManeuver(_maneuver);
        }
            
    }

    public void Cancel() => Halt();

    void Perform(bool pauseImmediately)
    {
       if (!pauseImmediately)
            _routine = StartCoroutine(CR_Reload());
    }

    void Pause() => Halt();

    void Resume() => _routine = StartCoroutine(CR_Reload());

    void Halt()
    {
        if (_routine != null)
            StopCoroutine(_routine);
        _maneuver.Dequeue();
    }

    IEnumerator CR_Reload()
    {
        yield return _reloadWait;

        foreach (AmmoType ammoType in WeaponHost.ExpectedAmmos)
        {
            StaticResource weaponAmmo = WeaponHost.Inventory.GetAmmo(ammoType);
            int missingAmount = weaponAmmo.Max - weaponAmmo.Current;

            StaticResource handlerAmmo = WeaponHost.Handler.Inventory.GetAmmo(ammoType);
            weaponAmmo.Fill(handlerAmmo.Drain(missingAmount));
        }

        Halt();
    }
}
