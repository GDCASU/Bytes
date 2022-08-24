using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastProjectile : Projectile
{
    [SerializeField]
    protected float maxRange;
    [SerializeField]
    private float width;

    protected float launchSpeed;
    protected float buttDistanceAlongRay = 0f;
    protected Vector3 bulletButt = Vector3.zero;
    protected Coroutine travelRoutine;
    protected WaitForFixedUpdate fixedWait = new WaitForFixedUpdate();

    public override void Launch(Ray ray, float launchSpeed, CharacterType targetType)
    {
        CommenceAging();
        this.targetType = targetType;

        RaycastHit hit;
        if (launchSpeed <= 0)
        {
            if (Physics.Raycast(ray, out hit, maxRange))
            {
                if (hit.transform.CompareTag(targetType.ToString()))
                {
                    hit.transform.GetComponent<Character>().ReceiveDamage(impactDamage);
                }
            }
            Perish();
        }
        else
        {
            this.ray = ray;
            bulletButt = ray.origin;
            this.launchSpeed = launchSpeed * Time.fixedDeltaTime;
            travelRoutine = StartCoroutine(Travel());
        }
    }

    protected virtual IEnumerator Travel()
    {
        while (true)
        {
            RaycastHit hit;
            if (
                width <= 0 ?
                Physics.Raycast(bulletButt, ray.direction, out hit, launchSpeed) :
                Physics.SphereCast(bulletButt, width, ray.direction, out hit, launchSpeed)
                )
                {
                if (hit.transform.CompareTag(targetType.ToString()))
                {
                    hit.transform.root.GetComponent<Character>().ReceiveDamage(impactDamage);
                }
                Perish();
            }

            buttDistanceAlongRay += launchSpeed;
            bulletButt = ray.GetPoint(buttDistanceAlongRay);

            yield return fixedWait;
        }
    }

    protected override void Perish()
    {
        base.Perish();

        if (travelRoutine != null)
        {
            StopCoroutine(travelRoutine);
            travelRoutine = null;
        }
    }
}