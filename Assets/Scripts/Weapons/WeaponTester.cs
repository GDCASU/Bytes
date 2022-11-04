using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponTester : MonoBehaviour
{
    [SerializeField] InputReader inputReader = default;
    Weapon weapon;

    private void Awake()
    {
        weapon = GetComponent<Weapon>();
    }

    private void OnEnable()
    {
        inputReader.Gameplay.UsePrimaryAbility.performed += Primary;
        inputReader.Gameplay.UsePrimaryAbility.canceled += Primary;
        inputReader.Gameplay.UseSecondaryAbility.performed += Secondary;
        inputReader.Gameplay.UseSecondaryAbility.canceled += Secondary;
        inputReader.Gameplay.UseTertiaryAbility.performed += Tertiary;
        inputReader.Gameplay.UseTertiaryAbility.canceled += Tertiary;
        inputReader.Gameplay.Reload.performed += Reload;
    }

    void OnDisable()
    {
        inputReader.Gameplay.UsePrimaryAbility.performed -= Primary;
        inputReader.Gameplay.UsePrimaryAbility.canceled -= Primary;
        inputReader.Gameplay.UseSecondaryAbility.performed -= Secondary;
        inputReader.Gameplay.UseSecondaryAbility.canceled -= Secondary;
        inputReader.Gameplay.UseTertiaryAbility.performed -= Tertiary;
        inputReader.Gameplay.UseTertiaryAbility.canceled -= Tertiary;
        inputReader.Gameplay.Reload.performed -= Reload;
    }

    void Primary(InputAction.CallbackContext context)
    {
        weapon.PerformPrimaryAttack(context.phase == InputActionPhase.Performed);
    }

    void Secondary(InputAction.CallbackContext context)
    {
        weapon.PerformSecondaryAttack(context.phase == InputActionPhase.Performed);
    }

    void Tertiary(InputAction.CallbackContext context)
    {
        weapon.PerformTertiaryAttack(context.phase == InputActionPhase.Performed);
    }

    void Reload(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            weapon.PerformReload();
    }
}
