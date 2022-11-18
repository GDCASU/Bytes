using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ProjectileVisual))]
public class Old_ProjectileL_VelocityRaycast : Old_Projectile
{
    [SerializeField]
    protected float width;

    protected float launchSpeed;
    protected Vector3 bulletButt = Vector3.zero;
    protected float buttDistanceAlongRay;
    protected WaitForFixedUpdate fixedWait = new WaitForFixedUpdate();

    public override void Launch(Ray ray, float launchSpeed, CharacterType targetType, Vector3 visualSpawnPosition)
    {
        CommenceAging();
        this.targetType = targetType;
        transform.rotation = Quaternion.LookRotation(ray.direction);

        this.ray = ray;
        bulletButt = ray.origin;
        buttDistanceAlongRay = 0f;
        this.launchSpeed = launchSpeed * Time.fixedDeltaTime;

        ProjectileVisualData data = new ProjectileVisualData();
        data.startPosition = visualSpawnPosition;
        visual.Play(data);

        StartCoroutine(Travel());
    }

    protected virtual IEnumerator Travel()
    {
        while (true)
        {
            RaycastHit hit;
            if
            (
                width <= 0 ?
                Physics.Raycast(bulletButt, ray.direction, out hit, launchSpeed, targetType.GetLayerMask() | Constants.LayerMask.Environment) :
                Physics.SphereCast(bulletButt, width, ray.direction, out hit, launchSpeed, targetType.GetLayerMask() | Constants.LayerMask.Environment)
            )
            {
                if (hit.transform.gameObject.layer == targetType.GetLayer())
                    hit.transform.root.GetComponent<Character>().ReceiveDamage(impactDamage);
                Perish();
            }

            buttDistanceAlongRay += launchSpeed;
            bulletButt = ray.GetPoint(buttDistanceAlongRay);
            transform.position = bulletButt;

            yield return fixedWait;
        }
    }

    protected override void Perish()
    {
        base.Perish();
        StopCoroutine(Travel());
        visual.Stop();
    }
}