using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerProjectile : Projectile
{
    private Rigidbody body;

    protected virtual void Awake()
    {
        gameObject.SetActive(false);
        body = GetComponent<Rigidbody>();
    }

    public override void Launch(Ray ray, float launchSpeed)
    {
        gameObject.SetActive(true);
        transform.rotation = Quaternion.LookRotation(ray.direction);
        body.AddForce(ray.direction * launchSpeed, ForceMode.VelocityChange);
    }

    protected virtual void Update()
    {
        age += Time.deltaTime;
        if (age >= lifetime) Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tags.Player) || other.CompareTag(Tags.Enemy))
        {
            other.transform.root.GetComponent<ICharacter>().ReceiveDamage(impactDamage);
        }
        Destroy(gameObject);
    }
}
