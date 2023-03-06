using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerControllerNew : MonoBehaviour
{
    PlayerInput _input;
    Rigidbody rb;

    [Header("Ground Check")]
    [SerializeField] float playerHieght;
    [SerializeField] LayerMask whatIsGround;
    bool grounded;

    private void Awake()
    {
        _input = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();

        rb.freezeRotation = true;
    }

    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHieght * 0.5f + 0.2f, whatIsGround);

        speedControl();
    }

    private void FixedUpdate()
    {
        handleMove();
        handleJump();
    }
}
