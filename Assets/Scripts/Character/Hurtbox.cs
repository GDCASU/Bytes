/*
 * Author: Cristion Dominguez
 * Date: 13 Jan. 2023
 */

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
