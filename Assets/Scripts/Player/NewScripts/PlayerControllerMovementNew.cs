using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerControllerNew
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
    bool canJump;

    [Header("Crounching")]
    [SerializeField] float crouchSpeed;
    [SerializeField] float crouchYScale;
    [SerializeField] float startYScale;

    Vector2 moveInput;
    float horizontalInput;
    float verticelInput;

    Vector3 moveDirection;

    public MovementState moveState;

    public enum MovementState
    {
        walking,
        sprinting,
        crounching,
        air,
        free
    }

    //Handleing the Movement State
    public void handleMoveState()
    {
        if (grounded && _input.IsCrouchPressed) {//crouching
            moveState = MovementState.crounching;
            moveSpeed = crouchSpeed;
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
        }
        else //free
        {
            moveState = MovementState.free;
        }
    }

    #region Moving
    private void handleMove()
    {
        moveInput = _input.MoveVector;

        horizontalInput = moveInput.x;
        verticelInput = moveInput.y;

        moveDirection = orientation.forward * verticelInput + orientation.right * horizontalInput;

        if (grounded) { //moving on ground
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        else if (!grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultplier, ForceMode.Force);
        }

        if (grounded)
        {
            rb.drag = groundDrag;
        }
        else rb.drag = 0f;
    }

    private void handleSpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        //limit velocity if needed
        if (flatVel.magnitude> moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
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
            Invoke(nameof(jump), jumpCooldown);
        }
    }
    private void jump()
    {
        //reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpPower, ForceMode.Impulse);
    }
    private void resetJump()
    {
        canJump = true;
    }
    #endregion

    #region Crouching
    private void handleCrouch()
    {
        if (moveState == MovementState.crounching)
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }
        else
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }
    }
    #endregion

}
