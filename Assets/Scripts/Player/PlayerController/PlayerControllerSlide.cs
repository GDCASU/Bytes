using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerController
{
    [System.Serializable]
    public class SlideVariables
    {
        public float velocityToSlide = 13;
        public float slideForce = 1.1f;
        public float downwardSlideForce = 1.05f;
        public float slidingFriction = 0.02f;
        public float downwardSlideAcceleration = -0.001f;
        [Range(0, 1)]
        public float slideControl = 0.025f;
    }
    public IEnumerator SlideCoroutine()
    {
        float angle = Vector3.Angle(rb.velocity, Vector3.up);
        friction = (angle > 90) ? slideVariables.downwardSlideAcceleration : slideVariables.slidingFriction;
        previousState = playerState;
        playerState = PlayerState.Sliding;
        totalVelocityToAdd += rb.velocity * ((angle > 90) ? slideVariables.downwardSlideForce : slideVariables.slideForce);
        maxVelocity = baseMovementVariables.maxWalkVelocity;
        isSprinting = false;
        while (rb.velocity.magnitude > maxVelocity)
        {
            if (playerState == PlayerState.Jumping) yield break;
            rb.velocity = newForwardandRight.normalized * rb.velocity.magnitude * slideVariables.slideControl + rb.velocity * (1f - slideVariables.slideControl);
            if (!isGrounded)
            {
                friction = baseMovementVariables.inAirFriction;
                previousState = PlayerState.Sliding;
                isSprinting = true;
                yield break;
            }
            //if (!crouchBuffer)
            //{
            //    if (rb.velocity.magnitude > maxWalkVelocity) isSprinting = true;
            //    previousState = playerState;
            //    playerState = PlayerState.Grounded;
            //    yield break;
            //}
            yield return fixedUpdate;
        }
        friction = baseMovementVariables.groundFriction;
        previousState = playerState;
        playerState = PlayerState.Grounded;
    }
}
