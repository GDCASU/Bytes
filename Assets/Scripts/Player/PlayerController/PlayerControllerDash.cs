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

        [Header("Ground Checking")]
        public Transform groundChecker;
        public float gCheckRadius = 0.1f;
        public LayerMask groundMask;

        [Header("Dash Variables")]
        public float dashForce = 10.0f;
        public float initialCooldown = 1.0f;
        public float groundedCooldown = 0.1f;
    }
    void DashInput()
    {
        // Press the right shift key to dash
        if(InputManager.PlayerActions.Dash.WasPerformedThisFrame() && dashVariables.canDash)
        {
            Vector3 dashDirection = new Vector3(0, 0, 1) * dashVariables.dashForce;
            float xDir = InputManager.PlayerActions.Move.ReadValue<Vector2>().x;
            float zDir = InputManager.PlayerActions.Move.ReadValue<Vector2>().y;

            // Dash forward by default if no movement keys are pressed. Otherwise, dash in the direction the player is moving.
            if (xDir != 0.0f || zDir != 0.0f) dashDirection = new Vector3(xDir, 0, zDir) * dashVariables.dashForce;

            this.GetComponent<Rigidbody>().AddRelativeForce(dashDirection, ForceMode.Impulse);
            print("Dash!"); // For debugging only
            dashVariables.canDash = false;
            StartCoroutine(InitialCooldown());
        }
    }

    /// <summary>
    /// Check if the player is on the ground after dashing and the initial cooldown passed.
    /// </summary>
    private void CheckGroundBeforeDash()
    {
        if (Physics.CheckSphere(dashVariables.groundChecker.position, dashVariables.gCheckRadius, dashVariables.groundMask))
        {
            print("On the ground!"); // For debugging only
            dashVariables.setUpDash = false;
            StartCoroutine(GroundedCooldown());
        }
    }

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
        yield return new WaitForSeconds(dashVariables.groundedCooldown);
        dashVariables.canDash = true;
        print("Player can dash again."); // For debugging only
    }
}
