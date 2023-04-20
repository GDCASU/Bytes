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

public class PlatformMover : MonoBehaviour
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
        platform.transform.position = pointA.position;

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

    // Sets the movePlatform bool to true and sets the inserted game object as a child to the platform
    public void MovePlatform(GameObject player)
    {
        movePlatform = true;
        player.transform.SetParent(platform);
    }

    public void Triggered(GameObject other)
    {
        Debug.Log("Trigger Enter");
        if (other.transform.root.gameObject.tag == "Player")
        {
            Debug.Log("Player Collision");
            if (CheckSendPlayer(other))
            {
                other.transform.root.SetParent(platform.transform);
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
        if (timer >= 1f || timer <= 0f)
        {
            speed *= -1;
            movePlatform = false;

            // Check platform's children for the player.
            for(int i = 0; i < platform.childCount; i++)
            {
                if(platform.GetChild(i).tag == "Player")
                {
                    platform.GetChild(i).SetParent(transform.root.parent);
                }
            }
        }
    }

    // Checks if the elevator is at the lower point and the player is within bounds to be carried
    private bool CheckSendPlayer(GameObject player)
    {
        bool returnVal; // = platform.position == pointA.position;  // Platform is at correct position
        returnVal = //returnVal &&
            player.transform.position.y > platform.position.y;  // Player is above platform
        returnVal = returnVal &&
            player.transform.position.x < platform.position.x + platform.localScale.x &&
            player.transform.position.x > platform.position.x - platform.localScale.x &&
            player.transform.position.z < platform.position.z + platform.localScale.z &&
            player.transform.position.z > platform.position.z - platform.localScale.z;
        Debug.Log(returnVal);
        return returnVal;
    }
}
