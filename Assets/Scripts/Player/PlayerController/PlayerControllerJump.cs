using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerController
{
    [System.Serializable]
    public class JumpVariables
    {
        public float jumpBuffer = .3f;
        public float jumpStrength = 6.5f;
        public float jumpStregthDecreaser = .05f;
        public float jumpInAirStrength = 0;
        public float jumpInAirControl = .1f;
        public float jumpingInitialGravity = -.3f;

        public float highestPointHoldTime = .05f;
        public float justJumpedCooldown = .1f;
        public float coyoteTime = 0.15f;

        public int inAirJumps = 0;
    }
    public void JumpInput()
    {
        if (_player.IsJumpPressed)
        {
            _jumpBuffer = jumpVariables.jumpBuffer;
        }
    }
    public void HandleJumpInput()
    {
        if (_jumpBuffer <= 0) _jumpBuffer = 0;
        if (playerState != PlayerState.Climbing)
        {
            if (_jumpBuffer > 0 && (isGrounded || _coyoteTimer > 0) && playerState != PlayerState.Jumping && (crouchMechanic ? crouchVariables.topIsClear : true))
                StartCoroutine(JumpCoroutine(false));
            else if (playerState == PlayerState.InAir && _inAirJumps > 0 && _jumpBuffer > 0)
            {
                _inAirJumps--;
                StartCoroutine(JumpCoroutine(true));
            }
        }
        if (_jumpBuffer > 0) _jumpBuffer -= Time.fixedDeltaTime;
    }
    private IEnumerator JumpCoroutine(bool inAirJump)
    {
        SetVariablesOnJump();
        previousState = playerState;
        playerState = PlayerState.Jumping;
        y = jumpVariables.jumpStrength;
        SetInitialGravity(jumpVariables.jumpingInitialGravity);
        totalVelocityToAdd += newForwardandRight;
        airControl = jumpVariables.jumpInAirControl;
        if (inAirJump)
        {
            if ((xDir != 0 || zDir != 0)) rb.velocity = newForwardandRight.normalized * ((currentForwardAndRight.magnitude < baseMovementVariables.maxSprintVelocity) ? baseMovementVariables.maxSprintVelocity : currentForwardAndRight.magnitude);
            else rb.velocity = Vector3.zero;
        }
        else rb.velocity -= rb.velocity.y * Vector3.up;
        while (rb.velocity.y >= 0f && playerState != PlayerState.Grounded)
        {
            y -= jumpVariables.jumpStregthDecreaser;
            totalVelocityToAdd += Vector3.up * y;
            yield return fixedUpdate;
        }
        if (playerState != PlayerState.Grounded)
        {
            _highestPointHoldTimer = jumpVariables.highestPointHoldTime;
            SetInitialGravity(0);
            rb.velocity -= Vector3.up * rb.velocity.y;
            while (_highestPointHoldTimer > 0)
            {
                _highestPointHoldTimer -= Time.fixedDeltaTime;
                yield return fixedUpdate;
            }
            SetInitialGravity(baseMovementVariables.initialGravity);
        }
        airControl = baseMovementVariables.inAirControl;
        if (rb.velocity.magnitude >= baseMovementVariables.maxSprintVelocity) isSprinting = true;
        previousState = playerState;
        if (!isGrounded) playerState = PlayerState.InAir;
    }
    public void SetVariablesOnJump()
    {
        _jumpBuffer = 0;
        _justJumpedCooldown = jumpVariables.justJumpedCooldown;
    }
}
