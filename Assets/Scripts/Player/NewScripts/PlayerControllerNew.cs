using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerControllerNew : MonoBehaviour
{
    PlayerInput _input;
    Rigidbody rb;

    [SerializeField] Transform orientation;

    [Header("Ground Check")]
    [SerializeField] float playerHieght;
    [SerializeField] LayerMask whatIsGround;
    bool grounded;

    private void Awake()
    {
        _input = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();

        rb.freezeRotation = true;

        startYScale = transform.localScale.y;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        Debug.Log("FPC Update Called");

        grounded = Physics.Raycast(transform.position, Vector3.down, playerHieght * 0.5f + 0.2f, whatIsGround);

        handleCamera();
        handleMoveState();
        handleSpeedControl();

        Debug.Log("Player Move Input: " + _input.LookVector.ToString());
    }

    private void FixedUpdate()
    {
        Debug.Log("FPC FixedUpdate Called");

        handleMove();
        handleJump();
        handleCrouch();
    }
}
