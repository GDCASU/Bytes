using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionProjectile : Projectile
{
    private Rigidbody body;

    protected override void Awake()
    {
        base.Awake();
        body = GetComponent<Rigidbody>();
    }

    public override void Launch(Ray ray, float launchSpeed, CharacterType targetType)
    {
        CommenceAging();
        this.targetType = targetType;

        transform.rotation = Quaternion.LookRotation(ray.direction);
        body.velocity = ray.direction * launchSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag(targetType.ToString()))
        {
            collision.transform.root.GetComponent<Character>().ReceiveDamage(impactDamage);
        }
        Perish();
    }
}