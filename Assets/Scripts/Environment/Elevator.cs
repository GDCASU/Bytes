/*
 * By: Aaron Huggins
 * 
 * Description:
 * Moves an elevator platform between points when called to do so.
 * 
 * Public Functions:
 *      void MovePlatform();            // Calls the program to move the platform.
 *      void Triggered(Collider other); // Checks if the other is in a suitable position to be moved with the platform
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    #region Variables

    [Header("Reference Objects")]
    [SerializeField] Transform platform;
    [SerializeField] Transform pointA;
    [SerializeField] Transform pointB;

    [Header("Math Utility")]
    [SerializeField] bool UseHeight;
    [SerializeField] float height;
    [SerializeField] float speed = 1f;

    // State controls
    private float timer = 0f;
    private bool movePlatform = false;

    #endregion

    #region UnityEvents

    private void Start()
    {
        // Place pointB "height" above pointA
        if (UseHeight) pointB.position = pointA.position + new Vector3(0f, height, 0f);
    }

    private void FixedUpdate()
    {
        if (movePlatform) HandleMovement();
    }

    #endregion

    // Sets the movePlatform bool to true. After reach an endpoint, movePlatform reverts to false.
    public void MovePlatform()
    {
        movePlatform = true;
    }

    public void Triggered(Collider other)
    {
        Debug.Log("Trigger Enter");
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Player Collision");
            if (CheckSendPlayer(other.gameObject))
            {
                MovePlatform();
            }
        }
    }

    // Contains the code to actually move the platform
    private void HandleMovement()
    {
        platform.position = Vector3.Lerp(pointA.position, pointB.position, timer);
        timer += speed * Time.deltaTime;

        // Set platform to go backwards
        if (timer >= 1f)
        {
            speed *= -1;
            movePlatform = false;
        }
    }

    // Checks if the elevator is at the lower point and the player is within bounds to be carried
    private bool CheckSendPlayer(GameObject player)
    {
        bool returnVal = platform.position == pointA.position;  // Platform is at correct position
        returnVal = returnVal &&
            player.transform.position.y > platform.position.y;  // Player is above platform
        returnVal = returnVal &&
            player.transform.position.x < transform.position.x + transform.localScale.x &&
            player.transform.position.x > transform.position.x - transform.localScale.x &&
            player.transform.position.z < transform.position.z + transform.localScale.z &&
            player.transform.position.z > transform.position.z - transform.localScale.z;

        return returnVal;
    }
}
