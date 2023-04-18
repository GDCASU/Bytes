using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    const string PLAYER_TAG = "Player";

    [Header("Door Objects")]
    public GameObject Door_L, Door_R;

    [Header("Colliders")]
    public Collider rigidbodyCollider;

    [Header("Debug")]
    public bool debug;

    private Room room;
    private bool doorState; // true for closed, false for opened

    void Awake()
    {
        room = GetComponentInParent<Room>();
    }

    void Start()
    {
        ToggleDoors(true);
    }

    public void ToggleDoors(bool toggle) // true for closed (disable), false for open (enable)
    {
        if (toggle)
        {
            Door_L.SetActive(true);
            Door_R.SetActive(true);
            rigidbodyCollider.gameObject.SetActive(true);
            doorState = true;
        }
        else
        {
            Door_L.SetActive(false);
            Door_R.SetActive(false);
            rigidbodyCollider.gameObject.SetActive(false);
            doorState = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (room.roomState != RoomState.Combat)
        {
            if (debug) Debug.Log("Entered Trigger");
            if (other.CompareTag(PLAYER_TAG))
            {
                ToggleDoors(false);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (debug) Debug.Log("Exited Trigger");
        if (doorState == false && other.CompareTag(PLAYER_TAG))
        {
            ToggleDoors(true);
        }
    }

}
