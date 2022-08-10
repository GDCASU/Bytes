using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastProjectile : Projectile
{
    [SerializeField]
    protected bool isInstant;
    [SerializeField]
    protected float maxRange;

    private float launchSpeed;
    protected float buttDistanceAlongRay = 0f;
    protected Vector3 bulletButt = Vector3.zero;

    protected virtual void Awake()
    {
        gameObject.SetActive(false);
    }

    public override void Launch(Ray ray, float launchSpeed)
    {
        RaycastHit hit;
        if (isInstant)
        {
            if (Physics.Raycast(ray, out hit, maxRange))
            {
                if (hit.transform.CompareTag("Player") || hit.transform.CompareTag("Enemy"))
                {
                    hit.transform.GetComponent<ICharacter>().TakeDamage(impactDamage);
                }
            }
            Destroy(this);
        }
        else
        {
            this.ray = ray;
            bulletButt = ray.origin;
            this.launchSpeed = launchSpeed * Time.fixedDeltaTime;
            gameObject.SetActive(true);
        }
    }

    protected virtual void Update()
    {
        age += Time.deltaTime;
        if (age >= lifetime) Destroy(gameObject);
    }

    protected virtual void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(bulletButt, ray.direction, out hit, launchSpeed))
        {
            if (hit.transform.CompareTag("Player") || hit.transform.CompareTag("Enemy"))
            {
                hit.transform.GetComponent<ICharacter>().TakeDamage(impactDamage);
            }
            Destroy(gameObject);
        }

        buttDistanceAlongRay += launchSpeed;
        bulletButt = ray.GetPoint(buttDistanceAlongRay);
    }
}