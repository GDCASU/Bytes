/*
 * Author: Cristion Dominguez
 * Date: ???
 */

using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoSingleton<InputManager>
{
    private static PlayerInput playerInput;
    private static PlayerInputActionAsset playerInputActionAsset;

    protected override void Awake()
    {
        base.Awake();
        if (!GameObject.FindGameObjectWithTag("Player").TryGetComponent(out playerInput))
            Debug.LogError("PlayerInput component was not found.");
        
        playerInputActionAsset = new PlayerInputActionAsset();
        playerInputActionAsset.Player.Enable();
    }

    public static PlayerInputActionAsset.PlayerActions PlayerActions => playerInputActionAsset.Player;

    public static string CurrentControlScheme => playerInput.currentControlScheme;
}

public static class InputActionExtension
{
    /// <summary>
    /// Returns whether the action is in the performed state, which is useful for determining if the button is still pressed after a Hold interaction triggers.
    /// </summary>
    /// <returns>Is the action in the performed state?</returns>
    public static bool IsPerforming(this InputAction action) => action.phase == InputActionPhase.Performed;
}

