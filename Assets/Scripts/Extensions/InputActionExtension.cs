using UnityEngine.InputSystem;

public static class InputActionExtension
{
    /// <summary>
    /// Returns whether the action is in the performed state, which is useful for determining if the button is still pressed after a Hold interaction triggers.
    /// </summary>
    /// <returns>Is the action in the performed state?</returns>
    public static bool IsPerforming(this InputAction action) => action.phase == InputActionPhase.Performed;
}