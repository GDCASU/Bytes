using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, ICharacter
{
    public void TakeDamage(float damage)
    {
        Debug.Log("Player has received " + damage + " damage.");
    }
}
