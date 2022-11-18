using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ProjectileVisual))]
public class Old_ProjectileL_InstantRaycast : Old_Projectile
{
    [SerializeField] protected float maxRange;
    [SerializeField] protected float width;

    protected override void Awake()
    {
        base.Awake();
        visual.Finished = Perish;
    }

    public override void Launch(Ray ray, float launchSpeed, CharacterType targetType, Vector3 visualSpawnPosition)
    {
        RaycastHit hit;
        Vector3 endLinePosition = ray.origin + (ray.direction * maxRange);
        if
        (
            width <= 0 ?
            Physics.Raycast(ray, out hit, maxRange, targetType.GetLayerMask() | Constants.LayerMask.Environment) && hit.transform.root.CompareTag(targetType.ToString()) :
            Physics.SphereCast(ray.origin, width, ray.direction, out hit, maxRange, targetType.GetLayerMask() | Constants.LayerMask.Environment) && hit.transform.root.CompareTag(targetType.ToString())
        )
        {
            hit.transform.root.GetComponent<Character>().ReceiveDamage(impactDamage);
            endLinePosition = hit.point;
        }

        ProjectileVisualData data = new ProjectileVisualData();
        data.startPosition = visualSpawnPosition;
        data.endPosition = endLinePosition;
        visual.Play(data);
    }

    protected override void Perish()
    {
        if (returnSelf != null)
            returnSelf(this);
        else
            Destroy(gameObject);
    }
}
