using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class InputManager : MonoBehaviour
{
    private static PlayerInput playerInput;
    private static PlayerInputActionAsset playerInputActionAsset;
    public static InputManager singleton;

    private void Awake()
    {
        if (singleton == null)
            singleton = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        playerInput = GetComponent<PlayerInput>();
        playerInputActionAsset = new PlayerInputActionAsset();
        playerInputActionAsset.Player.Enable(); //
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

