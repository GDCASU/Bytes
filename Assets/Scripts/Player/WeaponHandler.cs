/*
 * Author: Cristion Dominguez
 * Date: ???
 */

using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(IWeaponWielder))]
public class WeaponHandler : MonoBehaviour
{
    [field: SerializeField] public Transform WeaponContainer { get; private set; }
    [field: SerializeField] public Transform ProjectileSpawn { get; private set; }
    [SerializeField] int currentWeaponIndex;
    [SerializeField] Weapon[] weapons;

    public IWeaponWielder Wielder { get; private set; }

    Weapon currentWeapon;
    int maxWeapons = 0;
    int numOfWeapons = 0;

    private void Awake()
    {
        Wielder = GetComponent<IWeaponWielder>();
        Wielder.Enabled += Dev_Enable;
        Wielder.Disabled += Dev_Disable;
        Wielder.Started += Dev_Start;
    }

    void Dev_Start()
    {
        maxWeapons = weapons.Length;
        for (int i = 0; i < maxWeapons; i++)
        {
            Weapon weapon = weapons[i];
            if (weapon != null)
            {
                WeaponEquipData data = new WeaponEquipData();
                data.container = WeaponContainer;
                data.projectileSpawn = ProjectileSpawn;
                data.target = CharacterType.Enemy;  // [REPLACE]
                weapon.PrepareWeapon(data);
                weapon.gameObject.SetActive(false);
                numOfWeapons++;
            }
        }

        if (currentWeaponIndex >= maxWeapons)
            currentWeaponIndex = 0;
        SetCurrentWeapon(currentWeaponIndex);
    }

    public void Dev_Enable()
    {
        Wielder.PrimaryAttackPerformed += OnPrimaryAttackPerformed;
        Wielder.PrimaryAttackCanceled += OnPrimaryAttackCanceled;

        Wielder.SecondaryAttackPerformed += OnSecondaryAttackPerformed;
        Wielder.SecondaryAttackCanceled += OnSecondaryAttackCanceled;

        Wielder.TertiaryAttackPerformed += OnTertiaryAttackPerformed;
        Wielder.TertiaryAttackCanceled += OnTertiaryAttackCanceled;

        Wielder.UtilityPerformed += OnUtilityPerformed;
        Wielder.UtilityCanceled += OnUtilityCanceled;

        Wielder.SwitchWeaponPerformed += OnSwitchWeaponPerformed;
    }
    public void Dev_Disable()
    {
        Wielder.PrimaryAttackPerformed -= OnPrimaryAttackPerformed;
        Wielder.PrimaryAttackCanceled -= OnPrimaryAttackCanceled;

        Wielder.SecondaryAttackPerformed -= OnSecondaryAttackPerformed;
        Wielder.SecondaryAttackCanceled -= OnSecondaryAttackCanceled;

        Wielder.TertiaryAttackPerformed -= OnTertiaryAttackPerformed;
        Wielder.TertiaryAttackCanceled -= OnTertiaryAttackCanceled;

        Wielder.UtilityPerformed -= OnUtilityPerformed;
        Wielder.UtilityCanceled -= OnUtilityCanceled;

        Wielder.SwitchWeaponPerformed -= OnSwitchWeaponPerformed;
    }

    void SetCurrentWeapon(int weaponIndex)
    {
        if (weaponIndex < 0 || weaponIndex >= weapons.Length)
            return;

        currentWeapon = weapons[weaponIndex];
        if (currentWeapon)
            currentWeapon.gameObject.SetActive(true);
    }

    void SetNewWeapon(Weapon newWeapon, int weaponIndex)
    {
        WeaponEquipData data = new WeaponEquipData();
        data.container = WeaponContainer;
        data.projectileSpawn = ProjectileSpawn;
        data.target = CharacterType.Enemy; // [REPLACE]
        newWeapon.PrepareWeapon(data);
        weapons[weaponIndex] = newWeapon;
    }

    void OnPrimaryAttackPerformed() { }
    void OnPrimaryAttackCanceled() { }
    void OnSecondaryAttackPerformed() { }
    void OnSecondaryAttackCanceled() { }
    void OnTertiaryAttackPerformed() { }
    void OnTertiaryAttackCanceled() { }
    void OnUtilityPerformed () { }
    void OnUtilityCanceled() { }
    public void OnSwitchWeaponPerformed(int switchValue)
    {
        if (numOfWeapons <= 1 || switchValue < -1 || switchValue > weapons.Length) return;

        currentWeapon.gameObject.SetActive(false);

        if (switchValue == -1)
        {
            currentWeaponIndex = currentWeaponIndex - 1 >= 0 ? currentWeaponIndex - 1 : weapons.Length - 1;
        }
        else if (switchValue == 0)
        {
            currentWeaponIndex = currentWeaponIndex + 1 < weapons.Length ? currentWeaponIndex + 1 : 0;
        }
        else
        {
            currentWeaponIndex = switchValue - 1;
        }

        SetCurrentWeapon(currentWeaponIndex);
    }

    // [REMOVE]
    void OnBlock(InputAction.CallbackContext context) { if (currentWeapon) currentWeapon.Block(context.phase == InputActionPhase.Performed); }
    void OnReload(InputAction.CallbackContext context) { if (currentWeapon) currentWeapon.Reload(); }
    void OnShoot(InputAction.CallbackContext context) { if (currentWeapon) currentWeapon.Shoot(context.phase == InputActionPhase.Performed); }
    void OnStrike(InputAction.CallbackContext context) { if (currentWeapon) currentWeapon.Strike(); }

    public void TakeNewWeapon(Weapon newWeapon)
    {
        if (numOfWeapons < maxWeapons)
        {
            // Add new weapon to empty slot
            if (weapons[numOfWeapons] == null)
            {
                SetNewWeapon(newWeapon, numOfWeapons);
                if (currentWeapon)
                    newWeapon.gameObject.SetActive(false);
                else
                    SetCurrentWeapon(numOfWeapons);
                numOfWeapons++;
            }
        } 
        else
        {
            // Replace current weapon with the new one
            WeaponUnequipData data = new WeaponUnequipData();
            data.dropPosition = newWeapon.transform.position;
            data.dropRotation = newWeapon.transform.rotation;
            currentWeapon.NeglectWeapon(data);
            SetNewWeapon(newWeapon, currentWeaponIndex);
            SetCurrentWeapon(currentWeaponIndex);
        }
    }
}
