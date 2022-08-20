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
    
    private GameObject currentWeaponObject;
    private IWeapon[] weapons;
    private IWeapon currentWeapon;

    private int maxWeapons = 0;
    private int numOfWeapons = 0;

    private void Awake()
    {
        maxWeapons = weaponObjects.Length;
        weapons = new IWeapon[maxWeapons];
        for (int i = 0; i < maxWeapons; i++)
        {
            if (weaponObjects[i] != null)
            {
                weaponObjects[i].GetComponent<Collider>().enabled = false;
                weaponObjects[i].transform.position = handler.position;
                weaponObjects[i].transform.rotation = handler.rotation;
                weaponObjects[i].transform.parent = handler.transform;
                weapons[i] = weaponObjects[i].GetComponent<IWeapon>();
                weaponObjects[i].GetComponent<EquipableEntity>().Equip();
                numOfWeapons++;
            }
        }

        SetCurrentWeapon(0);
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

        currentWeaponObject.SetActive(false);

        if (context.ReadValue<float>() > 0)
        {
            currentWeaponIndex = currentWeaponIndex - 1 >= 0 ? currentWeaponIndex - 1 : weaponObjects.Length - 1;
        }
        else
        {
            currentWeaponIndex = currentWeaponIndex + 1 < weaponObjects.Length ? currentWeaponIndex + 1 : 0;
        }

        SetCurrentWeapon(currentWeaponIndex);
        currentWeaponObject.SetActive(true);
    }

    private void SetCurrentWeapon(int weaponIndex)
    {
        currentWeaponObject = weaponObjects[weaponIndex];
        currentWeapon = weapons[weaponIndex];
    }

    private void SetNewWeapon(GameObject newWeapon, int weaponIndex)
    {
        weaponObjects[weaponIndex] = newWeapon;
        weaponObjects[weaponIndex].transform.SetParent(handler);
        weaponObjects[weaponIndex].transform.localPosition = Vector3.zero;
        weaponObjects[weaponIndex].transform.localRotation = Quaternion.identity;
        weaponObjects[weaponIndex].GetComponent<Collider>().enabled = false;
        weaponObjects[weaponIndex].GetComponent<EquipableEntity>().Equip();

        weapons[weaponIndex] = weaponObjects[weaponIndex].GetComponent<IWeapon>();
    }

    private void OnBlock(InputAction.CallbackContext context) => currentWeapon.Block(context.phase == InputActionPhase.Performed);
    private void OnReload(InputAction.CallbackContext context) => currentWeapon.Reload();
    private void OnShoot(InputAction.CallbackContext context) => currentWeapon.Shoot(context.phase == InputActionPhase.Performed);
    private void OnStrike(InputAction.CallbackContext context) => currentWeapon.Strike();

    public void TakeNewWeapon(GameObject newWeapon)
    {
        if (numOfWeapons < maxWeapons)
        {
            // Add new weapon to empty slot
            for (int i = 0; i < maxWeapons; i++)
            {
                if (weaponObjects[i] == null)
                {
                    SetNewWeapon(newWeapon, i);
                    newWeapon.SetActive(false);
                    numOfWeapons++;
                }
            }
        } 
        else
        {
            // Replace current weapon with the new one
            // *Note: disabling and activating a gameobject shall ensure the animator does not mess with positioning when overriding parent transforms.
            currentWeaponObject.SetActive(false);
            currentWeaponObject.GetComponent<Collider>().enabled = true;
            currentWeaponObject.GetComponent<EquipableEntity>().Unequip();
            currentWeaponObject.transform.position = newWeapon.transform.position;
            currentWeaponObject.transform.rotation = newWeapon.transform.rotation;
            currentWeaponObject.transform.SetParent(null);
            currentWeaponObject.SetActive(true);

            newWeapon.SetActive(false);
            SetNewWeapon(newWeapon, currentWeaponIndex);
            SetCurrentWeapon(currentWeaponIndex);
            newWeapon.SetActive(true);
        }
    }
}
