using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerController
{
    [System.Serializable]
    public class BaseMovementVariables
    {
        #region Variables

        #region General
        [Header("General")]
        public float maxSlope = 60;
        public bool holdSprint;
        [HideInInspector] public float groundCheckDistance;
        #endregion

        #region Acceleration
        [Header("Acceleration")]
        public float walkSpeedIncrease = 1;
        public float sprintSpeedIncrease = 2;
        #endregion

        #region Velocity Caps
        [Header("Velocity Boundaries")]
        public float maxWalkVelocity = 7.5f;
        public float maxSprintVelocity = 15;
        public float minVelocity = .1f;
        #endregion

        #region Friction
        [Header("Friction Values")]
        public float noInputFriction = .2f;
        public float groundFriction = .1f;
        public float inAirFriction = .004f;
        #endregion

        #region In Air
        [Header("In Air Variables")]
        [Range(0, 1)]
        public float inAirControl = .021f;
        public float minAirVelocity = 2f;
        #endregion

        #region Gravity
        [Header("Gravity Variables")]
        public float initialGravity = -.55f;
        public float gravityRate = 1.008f;
        public float maxGravity = -39.2f;
        #endregion

        #region Fake Ground Checks
        [Header("Fake Ground Variables")]
        public float fakeGroundTime = .1f;
        [HideInInspector] public float _fakeGroundTimer;
        [HideInInspector] public bool feetSphereCheck;
        [HideInInspector] public bool kneesCheck;
        #endregion

        #region OriginalValues

        [HideInInspector] public float originalWalkingSpeed;
        [HideInInspector] public float originalSprintSpeed;
        #endregion

        #endregion

        public void StartVariables(CapsuleCollider capCollider, Transform transform) 
        {
            groundCheckDistance =
            ((capCollider.radius * transform.lossyScale.x * 2f) > (capCollider.height * transform.lossyScale.y)) ?
            0f : (capCollider.height * .5f * transform.lossyScale.y) - (capCollider.radius * transform.lossyScale.x);

            originalSprintSpeed = maxSprintVelocity;
            originalWalkingSpeed = maxWalkVelocity;
        } 
    }
    private void MovementInput()
    {
        //  Sprinting only disables when the player fully stops
        //if (InputManager.GetButtonDown(PlayerInput.PlayerButton.Sprint))
        //    if (crouchMechanic) isSprinting = (crouchVariables.isCrouching ? false : true);
        //    else isSprinting = true;

        if (baseMovementVariables.holdSprint)
        {
            isSprinting = _player.IsSprintPressed;
        }
        else if (_player.IsSprintPressed) isSprinting = !isSprinting;
        if (crouchMechanic) isSprinting = (crouchVariables.isCrouching ? false : isSprinting);

        speedIncrease = (isSprinting) ? baseMovementVariables.sprintSpeedIncrease : baseMovementVariables.walkSpeedIncrease;
        maxVelocity = (isSprinting) ? baseMovementVariables.maxSprintVelocity : baseMovementVariables.maxWalkVelocity;

        //if (Input.GetKey(KeyCode.W)) z = speedIncrease;
        //else if (Input.GetKey(KeyCode.S)) z = -speedIncrease;
        //else z = 0;
        //if (Input.GetKey(KeyCode.D)) x = speedIncrease;
        //else if (Input.GetKey(KeyCode.A)) x = -speedIncrease;
        //else x = 0;

        xDir = _player.MoveVector.x;
        zDir = _player.MoveVector.y;
    }
    private void GroundCheck()
    {
        stuckBetweenSurfacesHelper = 0;
        hit = new RaycastHit();
        currentForwardAndRight = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        if (jumpMechanic)
        {
            if (_coyoteTimer > 0) _coyoteTimer -= Time.fixedDeltaTime;
            if (jumpVariables.justJumpedCooldown > 0) _justJumpedCooldown -= Time.fixedDeltaTime;
        }

        RaycastHit[] hits = Physics.SphereCastAll(transform.position, capCollider.radius * transform.lossyScale.x, -transform.up,
           baseMovementVariables.groundCheckDistance + .01f * transform.lossyScale.y, ~ignores, QueryTriggerInteraction.Ignore);


        foreach (RaycastHit collision in hits)
        {
            if (collision.collider)
            {
                if (collision.point != Vector3.zero || (transform.position.x == 0 && transform.position.z == 0 ))
                {
                    float newSurfaceSlope = Vector3.Angle(collision.normal, Vector3.up);
                    if (!hit.collider)
                    {
                        hit = collision;
                        surfaceSlope = newSurfaceSlope;
                    }
                    else
                    {
                        if (newSurfaceSlope <= surfaceSlope)
                        {
                            hit = collision;
                            surfaceSlope = newSurfaceSlope;
                        }
                    }
                    if (newSurfaceSlope > baseMovementVariables.maxSlope) stuckBetweenSurfacesHelper++;
                }
            }
        }

        groundCheck = (!jumpMechanic || _justJumpedCooldown <= 0) ? (hit.collider) : false;
        if (surfaceSlope > baseMovementVariables.maxSlope)
        {
            groundCheck = false;
            if (playerState != PlayerState.Climbing && playerState != PlayerState.Jumping && playerState != PlayerState.InAir)
            {
                previousState = playerState;
                playerState = PlayerState.InAir;
                SetInitialGravity(baseMovementVariables.initialGravity);
            }
        }
        totalVelocityToAdd = Vector3.zero;
        newForwardandRight = Vector3.zero;

        groundedForward = Vector3.Cross(hit.normal, -transform.right);
        groundedRight = Vector3.Cross(hit.normal, transform.forward);

        //print(groundedForward + " " + hit.normal);
        //print(groundedRight + " " + hit.normal);

        //Change the value of the groundcheck if the player is on the fakeGround state
        if (onFakeGround)
        {
            if (groundCheck) onFakeGround = false;
            else
            {
                groundCheck = true;
                groundedForward = transform.forward;
                groundedRight = transform.right;
            }
        }
        //Player just landed
        if (groundCheck && (playerState == PlayerState.Jumping || playerState == PlayerState.InAir || playerState == PlayerState.Climbing))
        {
            rb.velocity = rb.velocity - Vector3.up * rb.velocity.y;
            float angleOfSurfaceAndVelocity = Vector3.Angle(rb.velocity, (hit.normal - Vector3.up * hit.normal.y));
            if (!onFakeGround && hit.normal.y != 1 && angleOfSurfaceAndVelocity < 5 && zDir > 0)
                rb.velocity = (groundedRight * xDir + groundedForward * zDir).normalized * rb.velocity.magnitude;          //This is to prevent the weird glitch where the player bounces on slopes if they land on them without jumping
            friction = baseMovementVariables.groundFriction;
            _inAirJumps = jumpVariables.inAirJumps;
            previousState = playerState;
            playerState = PlayerState.Grounded;
            if (playerJustLanded != null) playerJustLanded();
            PlayerLanded();
            SetInitialGravity(0);
        }
        //Player just left the ground
        if (isGrounded && !groundCheck)
        {
            if (playerState != PlayerState.Jumping)
            {
                previousState = playerState;
                playerState = PlayerState.InAir;
                SetInitialGravity(baseMovementVariables.initialGravity);
            }
            surfaceSlope = 0;
            friction = baseMovementVariables.inAirFriction;
            _coyoteTimer = jumpVariables.coyoteTime;
            if (playerLeftGround != null) playerLeftGround();
        }
        isGrounded = groundCheck;
        if (isGrounded)
        {
            if (xDir == 0 && zDir == 0) friction = baseMovementVariables.noInputFriction;
            else friction = baseMovementVariables.groundFriction;
        } 
        //If close to a small step, raise the player to the height of the step for a smoother feeling movement
        float maxDistance = capCollider.radius * (1 + ((isSprinting) ? (rb.velocity.magnitude / baseMovementVariables.maxSprintVelocity) : 0));

        if (playerState == PlayerState.Grounded) baseMovementVariables.feetSphereCheck = Physics.SphereCast(
            (transform.position + capCollider.center * capCollider.height * transform.lossyScale.y) -
            (transform.up * (transform.lossyScale.y * capCollider.height * .5f - capCollider.radius * transform.lossyScale.z)),
            capCollider.radius + .01f, rb.velocity.normalized, out feetHit, maxDistance, ~ignores);

        //print( transform.position + capCollider.center*capCollider.height*transform.lossyScale.y);
        if (baseMovementVariables.feetSphereCheck && !onFakeGround)
        {
            Vector3 direction = feetHit.point - (transform.position - Vector3.up * .5f * transform.lossyScale.y);
            float dist = direction.magnitude;
            //Debug.DrawLine(transform.position - Vector3.up * capCollider.height * .24f, (transform.position - Vector3.up * capCollider.height * .24f) + (direction - rb.velocity.y * Vector3.up));
            baseMovementVariables.kneesCheck = Physics.Raycast(transform.position - Vector3.up * capCollider.height * .24f, (direction - rb.velocity.y * Vector3.up), dist, ~ignores);
            if (!baseMovementVariables.kneesCheck && playerState == PlayerState.Grounded && (xDir != 0 || zDir != 0))
            {
                //StartCoroutine(FakeGround());
                isGrounded = true;
            }
            baseMovementVariables.kneesCheck = false;
        }
    }
    private void Move()
    {
        if (!isGrounded)//InAirMovement
        {
            if (playerState != PlayerState.Climbing && playerState != PlayerState.Vaulting)
            {
                rb.velocity -= currentForwardAndRight * friction;

                newForwardandRight = (transform.right * xDir + transform.forward * zDir);
                if (!Physics.Raycast(transform.position, newForwardandRight, capCollider.radius + 0.1f) && (xDir != 0 || zDir != 0))
                {
                    //If the game detects the player beeing stuck between two surfaces then it guarantees a min velocity to avoid a case where the stuck player's in air velocity would get stuck on zero 
                    Vector3 newVelocity = newForwardandRight.normalized *
                        (currentForwardAndRight.magnitude < .1f && stuckBetweenSurfacesHelper > 1 ? 1f : airControl) +
                        currentForwardAndRight;
                    if (newVelocity.magnitude < baseMovementVariables.minAirVelocity) newVelocity = newVelocity.normalized * baseMovementVariables.minAirVelocity;
                    rb.velocity = newVelocity + rb.velocity.y * Vector3.up;
                }
            }
        }
        else
        {
            newForwardandRight = (groundedRight.normalized * xDir + groundedForward.normalized * zDir);
            if (hit.normal.y == 1)
            {
                newForwardandRight = new Vector3(newForwardandRight.x, 0, newForwardandRight.z);
                rb.velocity = (rb.velocity - Vector3.up * rb.velocity.y).normalized * rb.velocity.magnitude;
            }

            if (rb.velocity.magnitude < maxVelocity)
            {
                totalVelocityToAdd += newForwardandRight;
            }
            else if (playerState != PlayerState.Sliding)
            {
                //If the palyer changes direction when going at the maxSpeed then decrease speed for smoother momentum shift
                if ((zDir == 0 && xDir == 0) || (pvX < 0 && xDir > 0)
                    || (xDir < 0 && pvX > 0) || (pvZ < 0 && zDir > 0)
                    || (zDir < 0 && pvZ > 0)) rb.velocity *= .99f; 
                else if (rb.velocity.magnitude < maxVelocity + 1f && (xDir!=0 || y!=0)) rb.velocity = newForwardandRight.normalized * maxVelocity;
                totalVelocityToAdd = Vector3.zero;
            }

            if (rb.velocity.magnitude != maxVelocity || (xDir == 0 && zDir == 0))
            {
                totalVelocityToAdd -= rb.velocity * friction;
            }

            pvX = xDir;
            pvZ = zDir;
        }
    }
    public void ToggleGravity(bool active)
    {
        previousState = playerState;
        playerState = PlayerState.InAir;
        if (active)
        {
            useGravity = true;
            SetInitialGravity(0);
        }
        else
        {
            useGravity = false;
            g = 0;
        }
    }
    public void SetInitialGravity(float value) => g = value;
    public void SetGravityRate(float value) => _gravityRate = value;
    private void ApplyGravity()
    {
        //if (playerState != PlayerState.Climbing)
        //{
        if (!isGrounded)
        {
            totalVelocityToAdd += Vector3.up * g;
        }
        if (g > baseMovementVariables.maxGravity) g *= _gravityRate;
        //}
    }

    private IEnumerator FakeGround()
    {
        onFakeGround = true;
        transform.position = new Vector3(transform.position.x, feetHit.point.y + capCollider.height * (crouchVariables.isCrouching ? 1f : .5f) * transform.lossyScale.y, transform.position.z);

        SetInitialGravity(0);
        baseMovementVariables._fakeGroundTimer = baseMovementVariables.fakeGroundTime;
        while (baseMovementVariables._fakeGroundTimer > 0 && onFakeGround)
        {
            baseMovementVariables._fakeGroundTimer -= Time.fixedDeltaTime;
            yield return fixedUpdate;
        }
        onFakeGround = false;
    }
    private void PlayerLanded()
    {
        climbVariables._climbingCooldown = 0;
        //lastViablePosition = transform.position;
    }
}
