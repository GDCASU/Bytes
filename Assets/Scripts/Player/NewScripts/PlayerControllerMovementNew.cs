using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerControllerNew
{
    [Header("Movement")]
    [SerializeField] float moveSpeed;
    [SerializeField] float groundDrag;
    [SerializeField] float airDrag;
    [SerializeField] Transform orientation;

    [SerializeField] float jumpPower;
    [SerializeField] float jumpCooldown;
    [SerializeField] float airMultplier;
    bool canJump;

    Vector2 moveInput;
    float horizontalInput;
    float verticelInput;

    Vector3 moveDirection;


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

    private void speedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        //limit velocity if needed
        if (flatVel.magnitude> moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

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

}
