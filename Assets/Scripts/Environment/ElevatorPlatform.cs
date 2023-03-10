using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorPlatform : MonoBehaviour
{
    [SerializeField] Elevator elevator;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") elevator.Triggered(other);
    }
}
