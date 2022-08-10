using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponHandler : MonoBehaviour
{
    [SerializeField]
    private Transform handler;
    [SerializeField]
    private int currentWeaponIndex = 0;
    [SerializeField]
    private GameObject[] weaponObjects;

    private IWeapon[] weapons;
    private IWeapon currentWeapon;

    private void Awake()
    {
        weapons = new IWeapon[weaponObjects.Length];
        for (int i = 0; i < weaponObjects.Length; i++)
            weapons[i] = weaponObjects[i].GetComponent<IWeapon>();

        currentWeapon = weapons[currentWeaponIndex];
    }

    private void OnEnable()
    {
        InputManager.PlayerActions.SwitchWeapon.performed += OnSwitchWeapon;

        InputManager.PlayerActions.Block.performed += OnBlock;
        InputManager.PlayerActions.Block.canceled += OnBlock;

        InputManager.PlayerActions.Reload.performed += OnReload;

        InputManager.PlayerActions.Shoot.performed += OnShoot;
        InputManager.PlayerActions.Shoot.canceled += OnShoot;

        InputManager.PlayerActions.Strike.performed += OnStrike;
    }
    private void OnDisable()
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
        if (weaponObjects.Length <= 1) return;

        weaponObjects[currentWeaponIndex].SetActive(false);

        if (context.ReadValue<float>() > 0)
        {
            currentWeaponIndex = currentWeaponIndex - 1 >= 0 ? currentWeaponIndex - 1 : weaponObjects.Length - 1;
        }
        else
        {
            currentWeaponIndex = currentWeaponIndex + 1 < weaponObjects.Length ? currentWeaponIndex + 1 : 0;
        }

        weaponObjects[currentWeaponIndex].SetActive(true);
        currentWeapon = weapons[currentWeaponIndex];
    }

    private void OnBlock(InputAction.CallbackContext context) => currentWeapon.Block(context.phase == InputActionPhase.Performed);
    private void OnReload(InputAction.CallbackContext context) => currentWeapon.Reload();
    private void OnShoot(InputAction.CallbackContext context) => currentWeapon.Shoot(context.phase == InputActionPhase.Performed);
    private void OnStrike(InputAction.CallbackContext context) => currentWeapon.Strike();
}
