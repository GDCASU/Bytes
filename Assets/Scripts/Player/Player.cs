using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, IWeaponWielder
{
    [SerializeField] InputReader _inputReader = default;
    [SerializeField] CombatantAllegiance _allegiance = CombatantAllegiance.Protagonist;

    public CombatantAllegiance Allegiance => _allegiance;

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
    public event Action UtilityPerformed;
    public event Action UtilityCanceled;
    public event Action<int> SwitchWeaponPerformed;

    private void OnEnable()
    {
        Enabled?.Invoke();

        _inputReader.Gameplay.UsePrimaryAbility.performed += OnPrimaryAttack;
        _inputReader.Gameplay.UsePrimaryAbility.canceled += OnPrimaryAttack;

        _inputReader.Gameplay.UseSecondaryAbility.performed += OnSecondaryAttack;
        _inputReader.Gameplay.UseSecondaryAbility.canceled += OnSecondaryAttack;

        _inputReader.Gameplay.UseTertiaryAbility.performed += OnTertiaryAttack;
        _inputReader.Gameplay.UseTertiaryAbility.canceled += OnTertiaryAttack;

        _inputReader.Gameplay.UseUtility.performed += OnUtility;
        _inputReader.Gameplay.UseUtility.canceled += OnUtility;

        _inputReader.Gameplay.SwitchWeapon.performed += OnSwitchWeapon;
    }

    private void OnDisable()
    {
        Disabled?.Invoke();

        _inputReader.Gameplay.UsePrimaryAbility.performed -= OnPrimaryAttack;
        _inputReader.Gameplay.UsePrimaryAbility.canceled -= OnPrimaryAttack;

        _inputReader.Gameplay.UseSecondaryAbility.performed -= OnSecondaryAttack;
        _inputReader.Gameplay.UseSecondaryAbility.canceled -= OnSecondaryAttack;

        _inputReader.Gameplay.UseTertiaryAbility.performed -= OnTertiaryAttack;
        _inputReader.Gameplay.UseTertiaryAbility.canceled -= OnTertiaryAttack;

        _inputReader.Gameplay.UseUtility.performed -= OnUtility;
        _inputReader.Gameplay.UseUtility.canceled -= OnUtility;

        _inputReader.Gameplay.SwitchWeapon.performed -= OnSwitchWeapon;
    }

    private void Start()
    {
        Started?.Invoke();
    }

    private void Update()
    {
        Updated?.Invoke();
    }

    void OnPrimaryAttack(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            PrimaryAttackPerformed?.Invoke();
        else if (context.phase == InputActionPhase.Canceled)
            PrimaryAttackCanceled?.Invoke();
    }

    void OnSecondaryAttack(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            SecondaryAttackPerformed?.Invoke();
        else if (context.phase == InputActionPhase.Canceled)
            SecondaryAttackCanceled?.Invoke();
    }

    void OnTertiaryAttack(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            TertiaryAttackPerformed?.Invoke();
        else if (context.phase == InputActionPhase.Canceled)
            TertiaryAttackCanceled?.Invoke();
    }

    void OnUtility(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            UtilityPerformed?.Invoke();
        else if (context.phase == InputActionPhase.Canceled)
            UtilityCanceled?.Invoke();
    }

    void OnSwitchWeapon(InputAction.CallbackContext context)
    {
        int switchValue = (int)context.ReadValue<float>();

        if (switchValue == -120)
            switchValue = -1;
        else if (switchValue == 120)
            switchValue = 0;

        SwitchWeaponPerformed?.Invoke(switchValue);
    }
}
