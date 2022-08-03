using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerController
{
    [System.Serializable]
    public class ClimbVariables
    {
        #region Climb Requirements
        [Header("Climbing Requirements")]
        public float maxNegativeVelocityToClimb = -45;
        public float climbingDuration = 1;
        public float climbingCooldown = 2;
        [HideInInspector] public float _climbingCooldown;
        #endregion

        #region Climb Variables
        [Header("Climbing Variables")]
        public float climbAcceleration = 1.11f;
        public float climbAccelerationDecreaser = .0235f;
        public float minVelocityToPreserveOriginalMomentum = 2;
        public float maxClimbingVelocity = 10;
        public float initialClimbingGravity = -.5f;
        public float climbingGravityRate = 1.001f;
        #endregion

        #region Strafe Variables
        [Header("Strafe Variables")]
        public float climbingStrafe = .3f;
        public float climbStrafeDecreaser = .001f;
        public float maxClimbStrafeVelocity = 5;
        public float climbingStrafeFriction = .01f;
        #endregion

        #region End Of Climb Variables
        [Header("End Of Climb Variables")]
        public float endOfClimbHoldTime = 0.05f;
        public float endOfClimbAirControlDuration = .5f;
        public float endOfClimbAirControl = .1f;
        public float endOfClimbJumpHeight = 8;
        public float endOfClimbSideStrength = 3;
        public float endOfClimbAwayFromWallStrength = 3;
        public float slopedMaxYVelocity = -2f;
        public float maxYVelocity = 0;
        #endregion

        #region WallJump Variables
        [Header("WallJump Variables")]
        public float wallJumpHeightStrenght = 5;
        public float wallJumpNormalStrength = 5;
        #endregion
    }

    public void HandleClimb()
    {
        if (climbVariables._climbingCooldown > 0) climbVariables._climbingCooldown -= Time.fixedDeltaTime;
        if (playerState == PlayerState.InAir && vaultVariables.forwardCheck     //Check that the player is in the air and there is a valid wall in front of their center
            && rb.velocity.y > climbVariables.maxNegativeVelocityToClimb           //Check that player is not falling too fast to be able to start climbing
            && (z > 0 || currentForwardAndRight.magnitude > 0f)                 //Check that the player pressed the forward input or that the current forwardAndRight is  bigger than 0
            && climbVariables._climbingCooldown <= 0)                           //Check that the climbing ability is not on cooldown
        {
            previousState = playerState;
            playerState = PlayerState.Climbing;
            StartCoroutine(ClimbCoroutine());
        }
    }
    private IEnumerator ClimbCoroutine()
    {
        if (jumpMechanic) _justJumpedCooldown = 0;
        float climbingTime = climbVariables.climbingDuration;
        float climbingStrafe = climbVariables.climbingStrafe;

        Physics.BoxCast(transform.position - transform.forward.normalized * capCollider.radius * .5f,
           Vector3.one * capCollider.radius, transform.forward, out forwardHit, Quaternion.identity, 1f, ~ignores);

        /* Get all the vetors based on the plane of the wall collided with */
        Vector3 playerOnWallRightDirection = Vector3.Cross(forwardHit.normal, Vector3.up).normalized;
        Vector3 upwardDirection = (surfaceSlope == 0) ? Vector3.up : -Vector3.Cross(hit.normal, playerOnWallRightDirection).normalized;

        //retain the players right momentum
        Vector3 originalHorizontalClimbingDirection = Vector3.Project(velocityAtCollision, playerOnWallRightDirection);
        rb.velocity = (originalHorizontalClimbingDirection.magnitude > climbVariables.minVelocityToPreserveOriginalMomentum) ? originalHorizontalClimbingDirection : Vector3.zero + rb.velocity.y * transform.up;

        SetInitialGravity(climbVariables.initialClimbingGravity);
        SetGravityRate(climbVariables.climbingGravityRate);

        float climbAcceleration = climbVariables.climbAcceleration;

        while (!isGrounded && vaultVariables.forwardCheck && playerState == PlayerState.Climbing && climbingTime > 0)
        {
            if (_jumpBuffer > 0)        //If a jump input was detected perform a wall jump
            {
                rb.velocity += Vector3.up * climbVariables.wallJumpHeightStrenght + forwardHit.normal * climbVariables.wallJumpNormalStrength;
                SetInitialGravity(jumpVariables.jumpingInitialGravity);
                SetVariablesOnJump();
                climbVariables._climbingCooldown = climbVariables.climbingCooldown;
                previousState = playerState;
                playerState = PlayerState.InAir;
                yield break;
            }
            //Add the up and side forces based on the players input if they are not past their max velocities
            rb.velocity += upwardDirection.normalized * ((z > 0 && rb.velocity.y < climbVariables.maxClimbingVelocity) ? climbAcceleration : 0f);
            rb.velocity += (currentForwardAndRight.magnitude < climbVariables.maxClimbStrafeVelocity) ? playerOnWallRightDirection * x * climbingStrafe : Vector3.zero - currentForwardAndRight * climbVariables.climbingStrafeFriction;
            climbAcceleration -= climbVariables.climbAccelerationDecreaser;
            climbingTime -= Time.fixedDeltaTime;
            climbingStrafe -= climbVariables.climbStrafeDecreaser;

            yield return fixedUpdate;
        }
        SetGravityRate(baseMovementVariables.gravityRate);
        //Exit early if performing a vault
        if (playerState == PlayerState.Vaulting) yield break;

        /*if the player is not going down at the end of the climb then hold their psotition for a brief moment to allow
         * them to aim where they want to go with the end of climb jump*/
        if (surfaceSlope != 0 ? rb.velocity.y > climbVariables.slopedMaxYVelocity : rb.velocity.y > climbVariables.maxYVelocity)
        {
            float highestPointHoldTimer = climbVariables.endOfClimbHoldTime;
            SetInitialGravity(0);
            rb.velocity -= Vector3.up * rb.velocity.y;
            while (highestPointHoldTimer > 0)
            {
                highestPointHoldTimer -= Time.fixedDeltaTime;
                yield return fixedUpdate;
            }
            float playerForwardXWallRightAngle = Vector3.Angle(transform.forward, playerOnWallRightDirection);
            rb.velocity += Vector3.up * climbVariables.endOfClimbJumpHeight +
                forwardHit.normal * climbVariables.endOfClimbAwayFromWallStrength +
                (playerForwardXWallRightAngle < 90 ? 1 : -1) * playerOnWallRightDirection * climbVariables.endOfClimbSideStrength;
        }
        //Reset and set the necessary variables 
        climbVariables._climbingCooldown = climbVariables.climbingCooldown;
        previousState = playerState;
        if (!isGrounded)
        {
            playerState = PlayerState.InAir;
            //Give the player better in air control for a brief moment to allow them to better decide where to go after the climb ends
            StartCoroutine(EndOfClimbAirControl());
        }
        else rb.velocity = -Vector3.up * rb.velocity.y;
        SetInitialGravity(baseMovementVariables.initialGravity);
    }
    private IEnumerator EndOfClimbAirControl()
    {
        airControl = climbVariables.endOfClimbAirControl;
        yield return new WaitForSeconds(climbVariables.endOfClimbAirControlDuration);
        airControl = baseMovementVariables.inAirControl;
    }
}
