using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponHandler : MonoBehaviour
{
    [field: SerializeField]
    public Transform WeaponContainer { get; private set; }
    [field: SerializeField]
    public Transform ProjectileSpawn { get; protected set; }
    [SerializeField]
    private int currentWeaponIndex;
    [SerializeField]
    private Weapon[] weapons;

    private Weapon currentWeapon;

    private int maxWeapons = 0;
    private int numOfWeapons = 0;

    private void Awake()
    {
        maxWeapons = weapons.Length;
        for (int i = 0; i < maxWeapons; i++)
        {
            Weapon weapon = weapons[i];
            if (weapon != null)
            {
                weapon.PrepareWeapon(this);
                weapon.gameObject.SetActive(false);
                numOfWeapons++;
            }
        }

        if (currentWeaponIndex >= maxWeapons)
            currentWeaponIndex = 0;
        SetCurrentWeapon(currentWeaponIndex);
    }

    public void Dev_OnEnable()
    {
        InputManager.PlayerActions.SwitchWeapon.performed += OnSwitchWeapon;

        InputManager.PlayerActions.Block.performed += OnBlock;
        InputManager.PlayerActions.Block.canceled += OnBlock;

        InputManager.PlayerActions.Reload.performed += OnReload;

        InputManager.PlayerActions.Shoot.performed += OnShoot;
        InputManager.PlayerActions.Shoot.canceled += OnShoot;

        InputManager.PlayerActions.Strike.performed += OnStrike;
    }
    public void Dev_OnDisable()
    {
        InputManager.PlayerActions.SwitchWeapon.performed -= OnSwitchWeapon;

        InputManager.PlayerActions.Block.performed -= OnBlock;
        InputManager.PlayerActions.Block.canceled -= OnBlock;

        InputManager.PlayerActions.Reload.performed -= OnReload;

        InputManager.PlayerActions.Shoot.performed -= OnShoot;
        InputManager.PlayerActions.Shoot.canceled -= OnShoot;

        InputManager.PlayerActions.Strike.performed -= OnStrike;
    }

    private void OnSwitchWeapon(InputAction.CallbackContext context)
    {
        if (numOfWeapons <= 1) return;

        currentWeapon.gameObject.SetActive(false);

        if (context.ReadValue<float>() > 0)
        {
            currentWeaponIndex = currentWeaponIndex - 1 >= 0 ? currentWeaponIndex - 1 : weapons.Length - 1;
        }
        else
        {
            currentWeaponIndex = currentWeaponIndex + 1 < weapons.Length ? currentWeaponIndex + 1 : 0;
        }

        SetCurrentWeapon(currentWeaponIndex);
    }

    private void SetCurrentWeapon(int weaponIndex)
    {
        currentWeapon = weapons[weaponIndex];
        currentWeapon.gameObject.SetActive(true);
    }

        private void SetNewWeapon(Weapon newWeapon, int weaponIndex)
    {
        newWeapon.PrepareWeapon(this);
        weapons[weaponIndex] = newWeapon;
    }

    private void OnBlock(InputAction.CallbackContext context) => currentWeapon?.Block(context.phase == InputActionPhase.Performed);
    private void OnReload(InputAction.CallbackContext context) => currentWeapon?.Reload();
    private void OnShoot(InputAction.CallbackContext context) => currentWeapon?.Shoot(context.phase == InputActionPhase.Performed);
    private void OnStrike(InputAction.CallbackContext context) => currentWeapon?.Strike();

    public void TakeNewWeapon(Weapon newWeapon)
    {
        if (numOfWeapons < maxWeapons)
        {
            // Add new weapon to empty slot
            if (weapons[numOfWeapons] == null)
            {
                SetNewWeapon(newWeapon, numOfWeapons);
                newWeapon.gameObject.SetActive(false);
                numOfWeapons++;
            }
        } 
        else
        {
            // Replace current weapon with the new one
            currentWeapon.NeglectWeapon(newWeapon.transform.position, newWeapon.transform.rotation);
            SetNewWeapon(newWeapon, currentWeaponIndex);
            SetCurrentWeapon(currentWeaponIndex);
        }
    }
}
