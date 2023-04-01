using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [Header("Modifiers")]
    [SerializeField] float Height;
    [SerializeField] float Speed = 1f;
    [SerializeField] bool ScanForGround = true;
    
    private RaycastHit groundHit;

    private bool move = false;
    private float timer = 0f;

    [Header("References")]
    [SerializeField] Transform origin;
    [SerializeField] Transform bottom;
    [SerializeField] Transform playerMover;

    [SerializeField] Transform elevator;

    private PlayerControllerNew localPlayer;

    #region Init

    // Sets the elevator transforms to their appropriate positions based on Height and ScanForGround
    private void Start()
    {
        if (ScanForGround) HandleGroundScan();
        else HandlePureHeight();
    }

    // Scans for if ground is within the appropriate height range and sets the elevator to be at the ground
    // if so. If not, handles the elevator as if pure height should be used.
    void HandleGroundScan()
    {
        Physics.Raycast(origin.position, Vector3.down, out groundHit);
        if (groundHit.distance > Height)
        {
            // Set scale
            Vector3 scale = elevator.localScale;
            Vector3 newScale = new Vector3(scale.x, groundHit.distance / 2, scale.z);
            elevator.localScale = newScale;

            // Set position
            elevator.position = new Vector3(origin.position.x, origin.position.y - newScale.y, origin.position.z);
            bottom.position = groundHit.point;
        }
        else HandlePureHeight();
    }

    // Sets transforms based on "Height" only.
    void HandlePureHeight()
    {
        // Set scale
        Vector3 newScale = new Vector3(elevator.localScale.x, Height / 2, elevator.localScale.z);
        elevator.localScale = newScale;

        // Set position
        elevator.position = new Vector3(origin.position.x, origin.position.y - newScale.y, origin.position.z);
        bottom.position = origin.position - (Vector3.down * Height);
    }

    #endregion

    #region Move Player

    // Moves player if necessary
    private void FixedUpdate()
    {
        if (move) HandleMovement();
    }

    // Activates when the player enters the elevator collider
    public void Triggered(GameObject other)
    {
        if (other.tag == "Player") MovePlayer(other);
    }

    public void UnTrigger(GameObject other)
    {
        if(other.tag == "Player")
        {
            move = false;
            localPlayer.transform.SetParent(transform.root.parent);
        }
    }

    // Handles the movement of the player using the playerMover object
    void HandleMovement()
    {
        // Player should go down
        if(localPlayer.moveState == PlayerControllerNew.MovementState.crounching) Speed = Speed > 0 ? -Speed : Speed;
        // Player should go up
        else Speed = Speed < 0 ? -Speed : Speed;

        timer += Speed * Time.deltaTime;

        // Ensure that timer is between 0 and 1
        if (timer > 1f)
        {
            timer = 1f;
            return;
        }
        if(timer < 0f)
        {
            timer = 0f;
            return;
        }

        // Move player
        playerMover.position = Vector3.Lerp(bottom.position, origin.position, timer);
    }

    // Tells elevator to move the player
    public void MovePlayer(GameObject player)
    {
        move = true;
        player.transform.SetParent(playerMover);
        localPlayer = player.GetComponent<PlayerControllerNew>();

        // Set playerMover position and gather percentage
        playerMover.localPosition = new Vector3(origin.position.x, (player.transform.localPosition.y - player.transform.localScale.y / 2), origin.position.z);
        timer = (playerMover.position.y - bottom.position.y) / (origin.position.y - bottom.position.y);
    }

    #endregion
}