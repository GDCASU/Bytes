/*
 * Author: Cristion Dominguez
 * Date: 4 Jan. 2023
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Vector2 LookVector { get; private set; }
    public Vector2 MoveVector { get; private set; }
    public bool IsSprintPressed { get; private set; }
    public bool IsJumpPressed { get; private set; }
    public bool IsCrouchPressed { get; private set; }

    InteractionHandler _interactionHandler;
    WeaponHandler _weaponHandler;
    AugmentationHandler _augmentationHandler;

    private void Awake()
    {
        _interactionHandler = GetComponent<InteractionHandler>();
        _weaponHandler = GetComponent<WeaponHandler>();
        _augmentationHandler = GetComponent<AugmentationHandler>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        LookVector = context.ReadValue<Vector2>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveVector = context.ReadValue<Vector2>();
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        IsSprintPressed = context.performed;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        IsJumpPressed = context.performed;
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        IsCrouchPressed = context.performed;
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
            _interactionHandler.AttemptInteraction();
    }

    public void OnPrimaryAttack(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            _weaponHandler.TriggerWeaponAbility(WeaponAbilityType.Primary, true);
        else if (context.phase == InputActionPhase.Canceled)
            _weaponHandler.TriggerWeaponAbility(WeaponAbilityType.Primary, false);
    }

    public void OnSecondaryAttack(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            _weaponHandler.TriggerWeaponAbility(WeaponAbilityType.Secondary, true);
        else if (context.phase == InputActionPhase.Canceled)
            _weaponHandler.TriggerWeaponAbility(WeaponAbilityType.Secondary, false);
    }

    public void OnTertiaryAttack(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            _weaponHandler.TriggerWeaponAbility(WeaponAbilityType.Tertiary, true);
        else if (context.phase == InputActionPhase.Canceled)
            _weaponHandler.TriggerWeaponAbility(WeaponAbilityType.Tertiary, false);
    }

    public void OnReload(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            _weaponHandler.TriggerWeaponAbility(WeaponAbilityType.Reload, true);
        else if (context.phase == InputActionPhase.Canceled)
            _weaponHandler.TriggerWeaponAbility(WeaponAbilityType.Reload, false);
    }

    public void OnSwitchWeapon(InputAction.CallbackContext context)
    {
        int switchValue = (int)context.ReadValue<float>();

        if (switchValue == -120)
            switchValue = -1;
        else if (switchValue == 120)
            switchValue = 0;

        _weaponHandler.SwitchWeapon(switchValue);
    }

    public void UseFirstAugmentation(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            _augmentationHandler.TriggerAugmentation(0, true);
        else if (context.phase == InputActionPhase.Canceled)
            _augmentationHandler.TriggerAugmentation(0, false);
    }

    public void UseSecondAugmentation(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            _augmentationHandler.TriggerAugmentation(1, true);
        else if (context.phase == InputActionPhase.Canceled)
            _augmentationHandler.TriggerAugmentation(1, false);
    }

    public void UseThirdAugmentation(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            _augmentationHandler.TriggerAugmentation(2, true);
        else if (context.phase == InputActionPhase.Canceled)
            _augmentationHandler.TriggerAugmentation(2, false);
    }

    public void Pause(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed)
            return;

        if (Time.timeScale == 1)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }
}
