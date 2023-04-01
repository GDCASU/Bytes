using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] PlatformMover platformMover;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") platformMover.Triggered(other);
    }
}
