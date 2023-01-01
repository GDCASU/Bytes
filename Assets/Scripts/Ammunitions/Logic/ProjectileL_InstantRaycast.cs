using UnityEngine;

public class ProjectileL_InstantRaycast : Projectile
{
    [SerializeField] float _lifespan;
    [SerializeField] float _maxRange;
    [SerializeField] float _width;

    public override float Lifespan => _lifespan;

    protected override void Awake()
    {
        base.Awake();
        visual.Finished = Perish;
    }

    public override void Launch(Ray ray, float launchSpeed, CombatantAllegiance targetAllegiance, Vector3 visualSpawnPosition)
    {
        RaycastHit hit;
        Vector3 endLinePosition = ray.origin + (ray.direction * _maxRange);
        if
        (
            _width <= 0 ?
            Physics.Raycast(ray, out hit, _maxRange, targetAllegiance.GetLayerMask() | Constants.LayerMask.Environment) && hit.transform.root.CompareTag(targetAllegiance.ToString()) :
            Physics.SphereCast(ray.origin, _width, ray.direction, out hit, _maxRange, targetAllegiance.GetLayerMask() | Constants.LayerMask.Environment) && hit.transform.root.CompareTag(targetAllegiance.ToString())
        )
        {
            hit.transform.GetComponent<Hurtbox>().Owner.GetComponent<Character>().ReceiveDamage(impactDamage);
            endLinePosition = hit.point;
        }

        ProjectileVisualData data = new ProjectileVisualData
        {
            startPosition = visualSpawnPosition,
            endPosition = endLinePosition
        };
        visual.Play(data);
    }
}
