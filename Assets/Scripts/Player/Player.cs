using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, ICharacter
{
    [SerializeField]
    private Transform[] detectionPoints;

    public Transform[] GetDetectionPoints() => detectionPoints;

    public void TakeDamage(float damage)
    {
        Debug.Log("Player has received " + damage + " damage.");
    }
}
