using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour, IEnemy
{
    public void ReceiveDamage(float damage)
    {
        print("Dummy has received " + damage + " damage.");
    }

    public void ReceiveHealth(float addedHealth)
    {
        print("Dummy has received " + addedHealth + " health.");
    }

    public Transform[] GetDetectionPoints() => new Transform[] { transform };
}
