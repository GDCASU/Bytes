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

        public Transform groundChecker;
        public LayerMask groundMask;
        public float gCheckRadius = 0.4f;

        public float dashForce = 10.0f;
        public float initialCooldown = 1.0f;
        public float groundedCooldown = 0.1f;
    }
    void DashInput()
    {
        // Press the right shift key to dash
        if(InputManager.PlayerActions.Dash.WasPerformedThisFrame() && dashVariables.canDash)
        {
            this.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, 0, dashVariables.dashForce), ForceMode.Impulse);
            print("Dash!"); // For debugging only
            dashVariables.canDash = false;
            StartCoroutine(InitialCooldown());
        }
    }

    private IEnumerator InitialCooldown()
    {
        yield return new WaitForSeconds(dashVariables.initialCooldown);
        dashVariables.setUpDash = true;
        print("Setting up dash..."); // For debugging only
    }

    private IEnumerator GroundedCooldown()
    {
        // Keep looping until the player is on the ground
        yield return new WaitForSeconds(dashVariables.groundedCooldown);
        dashVariables.canDash = true;
        print("Player can dash again."); // For debugging only
    }
}
