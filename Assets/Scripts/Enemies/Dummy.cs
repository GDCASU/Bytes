using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour, IEnemy
{
    public Transform[] GetDetectionPoints() => new Transform[] { transform };

    public void TakeDamage(float damage)
    {
        Debug.Log("Dummy has received " + damage + " damage.");
    }
}
