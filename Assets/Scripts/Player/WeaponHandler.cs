/*
 * Author: Cristion Dominguez
 * Date: 4 Jan. 2023
 */

using System;
using System.Collections;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    public event Action Started;

    [field: SerializeField] public Transform WeaponContainer { get; private set; }
    [field: SerializeField] public Transform ProjectileSpawn { get; private set; }
    public ManeuverQueue MQueue { get; private set; }
    public AmmoInventory Inventory { get; private set; }
    public Damageable Combatant { get; private set; }

    [SerializeField] int _currentWeaponIndex;
    [SerializeField] int _switchPriority;
    [SerializeField] float _switchDuration;
    [SerializeField] Weapon[] _weapons;
    Weapon _currentWeapon;
    int _maxWeapons;
    int _weaponCount;
    int _incomingWeaponIndex;
    Maneuver _switchManeuver;
    Coroutine _switchRoutine;
    WaitForSeconds _switchWait;

    private void Awake()
    {
        Combatant = GetComponent<Damageable>();
        MQueue = GetComponent<ManeuverQueue>();
        _switchManeuver = new Maneuver(
            "Weapon Switch",
            _switchPriority,
            (bool pauseImmediately) =>
            {
                if (!pauseImmediately)
                    _switchRoutine = StartCoroutine(CR_Switch());
            },
            () =>
            {
                if (_switchRoutine != null)
                    StopCoroutine(_switchRoutine);
                _switchManeuver.Dequeue();
            },
            () => { },
            () =>
            {
                if (_switchRoutine != null)
                    StopCoroutine(_switchRoutine);
                _switchManeuver.Dequeue();
            });
        _switchWait = new WaitForSeconds(_switchDuration);

        Inventory = GetComponent<AmmoInventory>();
    }

    private void OnValidate()
    {
        _switchWait = new WaitForSeconds(_switchDuration);
    }

    void Start()
    {
        _maxWeapons = _weapons.Length;
        for (int i = 0; i < _maxWeapons; i++)
            _weapons[i]?.Interact(gameObject);


        if (_weaponCount < 1)
            Debug.LogError("There should be at least one weapon in the Weapons array.");

        if (_currentWeaponIndex >= _maxWeapons)
            _currentWeaponIndex = 0;
        ReadyWeapon(_currentWeaponIndex);

        Started?.Invoke();
    }

    public void TriggerWeaponAbility(WeaponAbilityType type, bool isStarting) => _currentWeapon.TriggerAbility(type, isStarting);

    public void EquipWeapon(Weapon newWeapon)
    {
        int weaponIndex;
        bool isReplacing = false;
        if (_weaponCount < _maxWeapons)
        {
            weaponIndex = _weaponCount;
            _weaponCount++;
        }
        else
        {
            weaponIndex = _currentWeaponIndex;
            if (_weapons[weaponIndex])
            {
                Weapon oldWeapon = _weapons[weaponIndex];
                oldWeapon.transform.SetParent(null);
                oldWeapon.transform.position = newWeapon.transform.position;
                oldWeapon.transform.rotation = newWeapon.transform.rotation;
                oldWeapon.Unequip();
            }
            isReplacing = true;
        }

        newWeapon.transform.SetParent(WeaponContainer);
        newWeapon.transform.localPosition = newWeapon.EquipOffset;
        newWeapon.transform.localRotation = Quaternion.identity;
        newWeapon.Equip(gameObject);
        _weapons[weaponIndex] = newWeapon;

        if (isReplacing)
            newWeapon.Ready();
        else
            newWeapon.gameObject.SetActive(false);
    }

    void ReadyWeapon(int weaponIndex)
    {
        if (weaponIndex < 0 || weaponIndex >= _weapons.Length || !_weapons[weaponIndex])
            return;

        _currentWeaponIndex = weaponIndex;

        _currentWeapon?.Store();
        _currentWeapon?.gameObject.SetActive(false);

        _currentWeapon = _weapons[weaponIndex];
        _currentWeapon.gameObject.SetActive(true);
        _currentWeapon.Ready();
    }

    public void SwitchWeapon(int switchValue)
    {
        if (_weaponCount <= 1 || switchValue < -1 || switchValue > _weaponCount || _switchManeuver.InQueue)
            return;

        if (switchValue == -1)
            _incomingWeaponIndex = _currentWeaponIndex - 1 >= 0 ? _currentWeaponIndex - 1 : _weaponCount - 1;
        else if (switchValue == 0)
            _incomingWeaponIndex = _currentWeaponIndex + 1 < _weaponCount ? _currentWeaponIndex + 1 : 0;
        else
            _incomingWeaponIndex = switchValue - 1;

        MQueue.Enqueue(_switchManeuver);
    }

    IEnumerator CR_Switch()
    {
        yield return _switchWait;
        ReadyWeapon(_incomingWeaponIndex);
        _switchManeuver.Dequeue();
    }
}
