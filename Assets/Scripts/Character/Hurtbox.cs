using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurtbox : MonoBehaviour
{
    public Collider AttachedCollider { get; private set; }
    public Damageable Owner { get; private set; }

    private void Awake()
    {
        AttachedCollider = GetComponent<Collider>();
        Owner = GetComponentInParent<Damageable>();
    }
}
