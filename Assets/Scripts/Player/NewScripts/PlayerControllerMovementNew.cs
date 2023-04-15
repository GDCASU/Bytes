using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerController
{
    private float moveSpeed;

    [Header("Movement")]
    [SerializeField] float walkSpeed;
    [SerializeField] float sprintSpeed;
    [SerializeField] float groundDrag;
    [SerializeField] float airDrag;

    [Header("Jumping")]
    [SerializeField] float jumpPower;
    [SerializeField] float jumpCooldown;
    [SerializeField] float airMultplier;
    [SerializeField] float fallForce;
    bool canJump = true;

    [Header("Crounching")]
    [SerializeField] float crouchSpeed;
    [SerializeField] float crouchYScale;
    private float startYScale;

    [Header("Slope Movement")]
    [SerializeField] float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;

    Vector2 moveInput;
    float horizontalInput;
    float verticelInput;

    Vector3 moveDirection;

    private MovementState previousMovementState;
    public MovementState moveState;
    public bool tryingToCrouch = false;

    public enum MovementState
    {
        walking,
        sprinting,
        crouching,
        air,
        free
    }

    //Handleing the Movement State
    public void handleMoveState()
    { 
        previousMovementState = moveState;

        // Player is trying to crouch
        if (_input.IsCrouchPressed) tryingToCrouch = true;
        else tryingToCrouch = false;

        if (grounded && _input.IsCrouchPressed) {//crouching
            moveState = MovementState.crouching;
            moveSpeed = crouchSpeed;

            if (previousMovementState != moveState) { enterCrouch(); }

        }
        else if (grounded && _input.IsSprintPressed && _input.MoveVector.magnitude > 0) //sprinting
        {
            moveState = MovementState.sprinting;
            moveSpeed = sprintSpeed;
        }
        else if (grounded && _input.MoveVector.magnitude > 0)//walking
        { 
            moveState = MovementState.walking;
            moveSpeed = walkSpeed;
        }
        else if (!grounded)//in air
        {
            moveState = MovementState.air;
            handleAir();
        }
        else //free
        {
            moveState = MovementState.free;
        }

        // Debug.Log("Move State" + moveState);
    }

    #region Moving
    private void handleMove()
    {
        moveInput = _input.MoveVector;

        horizontalInput = moveInput.x;
        verticelInput = moveInput.y;

        moveDirection = orientation.forward * verticelInput + orientation.right * horizontalInput;

        //moveing player on slope
        if (onSlope() && !exitingSlope)
        { 
            rb.AddForce(getSlopeMoveDirection(moveDirection) * moveSpeed * 20f, ForceMode.Force);

            if (rb.velocity.y != 0)
            {
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }
        //Moving the Player
        else if (grounded) { //moving on ground
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        else if (!grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultplier, ForceMode.Force);
        }

        //Handling Drag
        if (grounded)
        {
            rb.drag = groundDrag;
        }
        else rb.drag = airDrag;

        rb.useGravity = !onSlope();
    }

    private void handleSpeedControl()
    {
        if (onSlope() && !exitingSlope)
        {
            if (rb.velocity.magnitude > moveSpeed)
            {
                rb.velocity = rb.velocity.normalized * moveSpeed;
            }
        }
        else {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            //limit velocity if needed
            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
    }
    #endregion

    #region Jumping
    private void handleJump()
    {
        if (_input.IsJumpPressed && canJump && grounded)
        {
            canJump = false;
            jump();
            Invoke(nameof(resetJump), jumpCooldown);
        }
    }
    private void jump()
    {
        exitingSlope = true;

        //reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpPower, ForceMode.Impulse);
    }
    private void resetJump()
    {
        exitingSlope = false;
        canJump = true;
    }
    #endregion

    #region Crouching
    private void enterCrouch()
    {
        moveState = MovementState.crouching;
        moveSpeed = crouchSpeed;

        transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
        rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        // Debug.Log("Adding Crouch Force");
    }

    private void handleCrouch()
    {
        if (moveState == MovementState.crouching)
        {
            /*
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
            Debug.Log("Adding Crouch Force");
            */
        }
        else
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }
    }

    private void exitCrouch()
    {
        transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
    }
    #endregion

    #region In Air

    private void handleAir()
    {
        if (rb.velocity.y <= 0)
        {
            rb.AddForce(Vector3.down * fallForce, ForceMode.Force);
        }
    }

    #endregion

    #region Slope Movement

    public Vector3 getSlopeMoveDirection(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
    }

    public bool onSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHieght * 0.5f + 1f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    #endregion

}
