using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerController : MonoBehaviour
{
    #region Variables

    Player _player;

    #region Movement Mechanics
    [Header("Additional Mechanics")]
    public bool jumpMechanic;
    public bool crouchMechanic;
    public bool vaultMechanic;
    private bool movementDisabled;
    #endregion

    #region Additional Mechanics Variables
    public BaseMovementVariables baseMovementVariables = new BaseMovementVariables();
    public CrouchVariables crouchVariables = new CrouchVariables();
    public SlideVariables slideVariables = new SlideVariables();
    public JumpVariables jumpVariables = new JumpVariables();
    public VaultVariables vaultVariables = new VaultVariables();
    public ClimbVariables climbVariables = new ClimbVariables();
    #endregion

    #region Player States
    [Header("Player States")]
    public bool isGrounded;
    bool groundCheck;
    public bool isSprinting;
    public bool onFakeGround;
    public PlayerState playerState;
    public PlayerState previousState;
    #endregion

    #region Primitive Variables
    private float xDir, zDir;
    private float g;
    private float pvX, pvZ;
    private float y;
    #endregion

    #region Global Variables

    #region Basic Movement
    private float surfaceSlope;
    private float maxVelocity;
    private float speedIncrease;
    private float friction;
    private float airControl;
    private float _gravityRate;
    [HideInInspector] public bool useGravity = true;
    #endregion

    #region Jump
    private float _jumpBuffer;
    private float _highestPointHoldTimer;
    private float _justJumpedCooldown;
    private float _coyoteTimer;
    private int _inAirJumps;
    #endregion

    #region InAirVariables
    private int stuckBetweenSurfacesHelper;
    #endregion

    #endregion

    #region Vectors
    Vector3 groundedForward;
    Vector3 groundedRight;

    Vector3 totalVelocityToAdd;
    Vector3 newForwardandRight;
    Vector3 currentForwardAndRight;
    Vector3 velocityAtCollision;

    Vector3 lastViablePosition;
    #endregion

    #region Raycast hits
    [HideInInspector] public RaycastHit hit;
    [HideInInspector] public RaycastHit feetHit;
    [HideInInspector] public RaycastHit forwardHit;
    [HideInInspector] public RaycastHit rayToGround;
    #endregion

    #region Events and delegates
    public delegate void PlayerBecameGrounded();
    public event PlayerBecameGrounded playerJustLanded;
    public delegate void PlayerLeftTheGround();
    public event PlayerLeftTheGround playerLeftGround;
    #endregion

    #region Components
    Rigidbody rb;
    CapsuleCollider capCollider;
    public PlayerCamera playerCamera;
    #endregion

    #region Other
    private WaitForFixedUpdate fixedUpdate;
    public LayerMask ignores;
    #endregion

    public enum PlayerState
    {
        NotMoving,
        Grounded,
        Sliding,
        Jumping,
        Climbing,
        Vaulting,
        InAir,
    };

    #endregion

    void Start()
    {
        _player = GetComponent<Player>();
        lastViablePosition = transform.position;
        capCollider = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();
        fixedUpdate = new WaitForFixedUpdate();
        friction = baseMovementVariables.inAirFriction;
        airControl = baseMovementVariables.inAirControl;
        SetInitialGravity(baseMovementVariables.initialGravity);
        _gravityRate = baseMovementVariables.gravityRate;
        playerState = PlayerState.InAir;
        baseMovementVariables.StartVariables(capCollider, transform);
        if (capCollider.radius * 2 * transform.lossyScale.x >=
            transform.lossyScale.y * capCollider.height) crouchMechanic = false;
        /*
        if (InputManager.CurrentControlScheme == "Gamepad")
        {
            baseMovementVariables.holdSprint = false;
            crouchVariables.holdCrouch = false;
        }
        */
    }

    void Update()
    {
        if (!movementDisabled)
        {
            if (crouchMechanic) CrouchInput();
            MovementInput();
            if (jumpMechanic) JumpInput();
        }
    }
    private void FixedUpdate()
    {
        transform.rotation = Quaternion.Euler(0f, playerCamera.transform.localEulerAngles.y, 0f);
        GroundCheck();
        Move();
        if (useGravity)
        {
            if (crouchMechanic) HandleCrouchInput();
            if (jumpMechanic) HandleJumpInput();
            if (vaultMechanic) ClimbChecks();
            ApplyGravity();
        }
        rb.velocity += totalVelocityToAdd;
        if (rb.velocity.magnitude < baseMovementVariables.minVelocity && xDir == 0 && zDir == 0 && (isGrounded))        //If the player stops moving set its maxVelocity to walkingSpeed and set its rb velocity to 0
        {
            rb.velocity = Vector3.zero;
            isSprinting = false;
        }
        if (stuckBetweenSurfacesHelper >= 2) rb.velocity -= rb.velocity.y * Vector3.up;     //Allows the palyer to slide around when stuck between two or more surfaces
        // if (vaultMechanic) ClimbChecks();
    }

    public void UpdateRespawnPoint() => lastViablePosition = transform.position;
    /// <summary>
    /// Reset the players position to the one set by a checkpoint
    /// </summary>
    public void ResetPosition()
    {
        rb.velocity = Vector3.zero;
        SetInitialGravity(0);
        transform.position = lastViablePosition;
        previousState = playerState;
        playerState = PlayerState.InAir;
        g = baseMovementVariables.initialGravity;
    }
    public void ChangeWalkingSpeed(float newWalkingSpeed) =>baseMovementVariables.maxWalkVelocity = newWalkingSpeed;
    public void ResetWalkingSpeed() => baseMovementVariables.maxWalkVelocity = baseMovementVariables.originalWalkingSpeed;
    public void ChangeSprintSpeed(float newSprintSpeed) => baseMovementVariables.maxSprintVelocity = newSprintSpeed;
    public void ResetSprintSpeed() => baseMovementVariables.maxSprintVelocity = baseMovementVariables.originalSprintSpeed;
    public void DisableMovement()
    {
        xDir = 0;
        zDir = 0;
        movementDisabled = true;
    }
    public void EnableMovement() => movementDisabled = false;
}
