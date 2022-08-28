using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerController
{
    [System.Serializable]
    public class DashVariables
    {
        [HideInInspector] public bool canDash = true;
        [HideInInspector] public bool setUpDash = false;

        [Header("Dash Variables")]
        public float groundedDashForce = 15.0f;
        public float inAirDashForce = 6.0f;
        public float initialCooldown = 1.0f;
        public float groundedCooldown = 0.1f;
    }

    private bool isDashEnabled = false;

    void DashInput()
    {
        // Press the right shift key to dash
        if(isDashEnabled && InputManager.PlayerActions.Dash.WasPerformedThisFrame() && dashVariables.canDash)
        {
            Vector3 dashDirection = new Vector3(0, 0, 1);
            float xDir = InputManager.PlayerActions.Move.ReadValue<Vector2>().x;
            float zDir = InputManager.PlayerActions.Move.ReadValue<Vector2>().y;

            // Dash forward by default if no movement keys are pressed. Otherwise, dash in the direction the player is moving.
            if (xDir != 0.0f || zDir != 0.0f) dashDirection = new Vector3(xDir, 0, zDir);

            dashDirection *= (isGrounded ? dashVariables.groundedDashForce : dashVariables.inAirDashForce);

            this.GetComponent<Rigidbody>().AddRelativeForce(dashDirection, ForceMode.Impulse);
            print("Dash " + (isGrounded ? "on ground!" : "in air!")); // For debugging only
            dashVariables.canDash = false;
            StartCoroutine(InitialCooldown());
        }
    }

    public void EnableDash(bool isEnabled) => isDashEnabled = isEnabled;

    /// <summary>
    /// Initial cooldown that happens after the player dashes.
    /// </summary>
    private IEnumerator InitialCooldown()
    {
        yield return new WaitForSeconds(dashVariables.initialCooldown);
        dashVariables.setUpDash = true;
        print("Setting up dash..."); // For debugging only
    }

    /// <summary>
    /// The second cooldown once the player is on the ground.
    /// </summary>
    private IEnumerator GroundedCooldown()
    {
        dashVariables.setUpDash = false;
        yield return new WaitForSeconds(dashVariables.groundedCooldown);
        dashVariables.canDash = true;
        print("Player can dash again."); // For debugging only
    }
}
