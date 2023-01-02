using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, IWeaponWielder
{
    [SerializeField] CombatantAllegiance _allegiance = CombatantAllegiance.Protagonist;
    public CombatantAllegiance Allegiance => _allegiance;

    PlayerInput _input;
    public Vector2 LookVector { get; private set; }
    public Vector2 MoveVector { get; private set; }
    public bool IsSprintPressed { get; private set; }
    public bool IsJumpPressed { get; private set; }
    public bool IsCrouchPressed { get; private set; }
    public bool IsDashPressed { get; private set; }
    public bool IsInteractPressed { get; private set; }

    public event Action Enabled;
    public event Action Disabled;
    public event Action Started;
    public event Action Updated;
    public event Action PrimaryAttackPerformed;
    public event Action PrimaryAttackCanceled;
    public event Action SecondaryAttackPerformed;
    public event Action SecondaryAttackCanceled;
    public event Action TertiaryAttackPerformed;
    public event Action TertiaryAttackCanceled;
    public event Action ReloadPerformed;
    public event Action ReloadCanceled;
    public event Action<int> SwitchWeaponPerformed;

    private void OnEnable()
    {
        Enabled?.Invoke();
    }

    private void OnDisable()
    {
        Disabled?.Invoke();
    }

    private void Start()
    {
        Started?.Invoke();
    }

    private void Update()
    {
        Updated?.Invoke();
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

    public void OnDash(InputAction.CallbackContext context)
    {
        IsDashPressed = context.performed;
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        IsInteractPressed = context.performed;
    }

    public void OnPrimaryAttack(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            PrimaryAttackPerformed?.Invoke();
        else if (context.phase == InputActionPhase.Canceled)
            PrimaryAttackCanceled?.Invoke();
    }

    public void OnSecondaryAttack(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            SecondaryAttackPerformed?.Invoke();
        else if (context.phase == InputActionPhase.Canceled)
            SecondaryAttackCanceled?.Invoke();
    }

    public void OnTertiaryAttack(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            TertiaryAttackPerformed?.Invoke();
        else if (context.phase == InputActionPhase.Canceled)
            TertiaryAttackCanceled?.Invoke();
    }

    public void OnReload(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            ReloadPerformed?.Invoke();
        else if (context.phase == InputActionPhase.Canceled)
            ReloadCanceled?.Invoke();
    }

    public void OnSwitchWeapon(InputAction.CallbackContext context)
    {
        int switchValue = (int)context.ReadValue<float>();

        if (switchValue == -120)
            switchValue = -1;
        else if (switchValue == 120)
            switchValue = 0;

        SwitchWeaponPerformed?.Invoke(switchValue);
    }
}
