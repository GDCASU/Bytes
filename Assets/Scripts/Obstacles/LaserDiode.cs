using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDiode : MonoBehaviour
{
    public float damage = 1.0f;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") &&
            other.gameObject.GetComponent<Player>())
        {
            other.gameObject.GetComponent<Player>().TakeDamage(damage);
        }
    }
}
