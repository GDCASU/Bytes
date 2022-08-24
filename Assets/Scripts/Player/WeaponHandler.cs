using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponHandler : MonoBehaviour
{
    [SerializeField]
    private Transform handler;
    [SerializeField]
    private int currentWeaponIndex;
    [SerializeField]
    private IWeapon[] weapons;

    private IWeapon currentWeapon;

    private int maxWeapons = 0;
    private int numOfWeapons = 0;

    private void Awake()
    {
        maxWeapons = weapons.Length;
        for (int i = 0; i < maxWeapons; i++)
        {
            IWeapon weapon = weapons[i];
            if (weapon != null)
            {
                weapon.GetComponent<Collider>().enabled = false;
                weapon.transform.position = handler.position;
                weapon.transform.rotation = handler.rotation;
                weapon.transform.parent = handler.transform;
                weapon.GetComponent<EquipableEntity>().Equip();
                numOfWeapons++;
            }
        }

        if (currentWeaponIndex >= maxWeapons)
            currentWeaponIndex = 0;
        SetCurrentWeapon(currentWeaponIndex);
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
        currentWeapon.gameObject.SetActive(true);
    }

    private void SetCurrentWeapon(int weaponIndex) => currentWeapon = weapons[weaponIndex];

    private void SetNewWeapon(IWeapon newWeapon, int weaponIndex)
    {
        newWeapon.transform.SetParent(handler);
        newWeapon.transform.localPosition = Vector3.zero;
        newWeapon.transform.localRotation = Quaternion.identity;
        newWeapon.GetComponent<Collider>().enabled = false;
        newWeapon.GetComponent<EquipableEntity>().Equip();
        weapons[weaponIndex] = newWeapon;
    }

    private void OnBlock(InputAction.CallbackContext context) => currentWeapon.Block(context.phase == InputActionPhase.Performed);
    private void OnReload(InputAction.CallbackContext context) => currentWeapon.Reload();
    private void OnShoot(InputAction.CallbackContext context) => currentWeapon.Shoot(context.phase == InputActionPhase.Performed);
    private void OnStrike(InputAction.CallbackContext context) => currentWeapon.Strike();

    public void TakeNewWeapon(IWeapon newWeapon)
    {
        if (numOfWeapons < maxWeapons)
        {
            // Add new weapon to empty slot
            for (int i = 0; i < maxWeapons; i++)
            {
                if (weapons[i] == null)
                {
                    SetNewWeapon(newWeapon, i);
                    newWeapon.gameObject.SetActive(false);
                    numOfWeapons++;
                }
            }
        } 
        else
        {
            // Replace current weapon with the new one
            // *Note: disabling and activating a gameobject shall ensure the animator does not mess with positioning when overriding parent transforms.
            currentWeapon.gameObject.SetActive(false);
            currentWeapon.GetComponent<Collider>().enabled = true;
            currentWeapon.GetComponent<EquipableEntity>().Unequip();
            currentWeapon.transform.position = newWeapon.transform.position;
            currentWeapon.transform.rotation = newWeapon.transform.rotation;
            currentWeapon.transform.SetParent(null);
            currentWeapon.gameObject.SetActive(true);

            newWeapon.gameObject.SetActive(false);
            SetNewWeapon(newWeapon, currentWeaponIndex);
            SetCurrentWeapon(currentWeaponIndex);
            newWeapon.gameObject.SetActive(true);
        }
    }
}
