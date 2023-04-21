using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Elevator : MonoBehaviour
{
    [Header("Debug")]
    bool debug = false;

    [Header("Modifiers")]
    [SerializeField] float Height;
    [SerializeField] float Speed = 1f;
    [SerializeField] bool ScanForGround = true;
    [SerializeField] int Num_IgnoreWalls = 0;

    private RaycastHit[] hitList;
    private RaycastHit groundHit;

    private bool move = false;
    private float timer = 0f;

    [Header("References")]
    [SerializeField] Transform origin;
    [SerializeField] Transform bottom;
    [SerializeField] Transform playerMover;
    [SerializeField] GameObject invisibleBox;

    [SerializeField] Transform elevator;
    private PlayerController localPlayer;
    private GameObject PlayerRoot;

    #region Init

    // Sets the elevator transforms to their appropriate positions based on Height and ScanForGround
    private void Start()
    {
        invisibleBox.SetActive(false);
        invisibleBox.transform.localPosition *= 1 / elevator.lossyScale.y;
        invisibleBox.transform.localScale = new Vector3(invisibleBox.transform.localScale.x, invisibleBox.transform.localScale.y / elevator.lossyScale.y, invisibleBox.transform.localScale.z);
        if (invisibleBox.transform.position.y > playerMover.position.y)
        {
            if (debug) Debug.Log(invisibleBox.transform.localPosition);
            invisibleBox.transform.localPosition = 
                new Vector3(
                invisibleBox.transform.localPosition.x,
                invisibleBox.transform.localPosition.y * -1,
                invisibleBox.transform.localPosition.z);
            if (debug) Debug.Log("Greater | " + invisibleBox.transform.localPosition);
        }

        Debug.Log(invisibleBox.transform.position +
            "\n" + playerMover.position);

        if (ScanForGround) HandleGroundScan();
        else HandlePureHeight();
    }

    // Scans for if ground is within the appropriate height range and sets the elevator to be at the ground
    // if so. If not, handles the elevator as if pure height should be used.
    void HandleGroundScan()
    {
        // Shoot a raycast and choose first non-hatch
        Ray castDir = new Ray(origin.position, Vector3.down);
        hitList = Physics.RaycastAll(castDir);

        groundHit = hitList[Num_IgnoreWalls];

        if (groundHit.distance < Height)
        {
            // Gather current scaling information
            Vector3 localScale = elevator.localScale;
            Vector3 lossyScale = elevator.lossyScale;
            Vector3 externalScale = new Vector3(lossyScale.x / localScale.x, lossyScale.y / localScale.y, lossyScale.z / localScale.z);

            // Construct new scaling information (only y scale affected)
            /*
             * Mathematical Formula:
             * Since a capsule's    height = (scale.y * 2),
             * we can find that     scale.y = desired_height / 2
             * However, the external scale affects the height. 
             * It does this by multiplying every part of the transform of a 
             * child object by the scaling of the parent object.
             * So, the scale looks like this:
             *                      ActualScale = LocalScale * ExternalScale
             * In order to account for this, the scale must be divided by the ExternalScale, looking something like
             *      ActualScale = (LocalScale / ExternalScale) * ExternalScale.
             * Then the LocalScale will be equivalent to the ActualScale.
             */
            float scaleY = ((groundHit.distance / externalScale.y) / 2);
            Vector3 newScale = new Vector3(localScale.x, scaleY, localScale.z);
            elevator.localScale = newScale;

            // Set position
            elevator.position = new Vector3(origin.position.x, origin.position.y - (groundHit.distance / 2), origin.position.z);
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

    #region Events

    // Moves player if necessary
    private void FixedUpdate()
    {
        if (move) HandleMovement();
    }

    // Activates when the player enters the elevator collider
    public void Triggered(GameObject other)
    {
        if (other.transform.root.tag == "Player") MovePlayer(other);
    }

    public void UnTrigger(GameObject other)
    {
        if(other.transform.parent.gameObject == localPlayer.gameObject)
        {
            move = false;
            PlayerRoot.transform.SetParent(transform.root.parent);
            invisibleBox.SetActive(false);
        }
    }

    #endregion

    #region Move Player

    // Tells elevator to move the player
    public void MovePlayer(GameObject player)
    {
        // Set start position
        playerMover.position = new Vector3(origin.position.x, player.transform.position.y, origin.position.z);

        // Set References
        PlayerRoot = player.transform.root.gameObject;
        player.transform.root.SetParent(playerMover);
        localPlayer = player.GetComponentInParent<PlayerController>();

        // Set States
        move = true;
        timer = (playerMover.position.y - bottom.position.y) / (origin.position.y - bottom.position.y);
        invisibleBox.SetActive(true);
    }

    // Handles the movement of the player using the playerMover object
    void HandleMovement()
    {
        // Player should go down
        if(localPlayer.tryingToCrouch) Speed = Speed > 0 ? -Speed : Speed;
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

    #endregion
}