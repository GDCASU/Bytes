using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorSupport : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Elevator elevator;

    private void OnTriggerEnter(Collider other)
    {
        elevator.Triggered(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        elevator.UnTrigger(other.gameObject);
    }
}
