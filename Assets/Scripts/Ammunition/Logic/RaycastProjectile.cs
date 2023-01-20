/*
 * Author: Cristion Dominguez
 * Date: 13 Jan. 2023
 */

using System.Collections;
using UnityEngine;

public class RaycastProjectile : Projectile
{
    [SerializeField] float _width;
    [SerializeField] bool _isInstant;
    [SerializeField] float _lifespan;
    [SerializeField] float _maxRange;
    Coroutine _travelRoutine;

    public override float Lifespan => _lifespan;

    protected override void Awake()
    {
        base.Awake();
        visual.Finished += AttemptPerish;
    }

    public override void Launch(Ray ray, float launchSpeed, CharacterAllegiance targetAllegiance, Vector3 visualSpawnPosition)
    {
        if (_isInstant)
        {
            RaycastHit hit;
            Vector3 endLinePosition = ray.origin + (ray.direction * _maxRange);
            if
            (
                _width <= 0 ?
                Physics.Raycast(ray, out hit, _maxRange, targetAllegiance.GetLayerMask() | Constants.LayerMask.Environment) && hit.transform.gameObject.layer == targetAllegiance.GetLayer() :
                Physics.SphereCast(ray.origin, _width, ray.direction, out hit, _maxRange, targetAllegiance.GetLayerMask() | Constants.LayerMask.Environment) && hit.transform.gameObject.layer == targetAllegiance.GetLayer()
            )
            {
                hit.transform.GetComponent<Hurtbox>().Owner.ReceiveDamage(impactDamage);
                endLinePosition = hit.point;
            }

            ProjectileVisualData data = new()
            {
                startPosition = visualSpawnPosition,
                endPosition = endLinePosition
            };
            visual.Play(data);
        }
        else
        {
            CommenceAging();
            transform.position = ray.origin;
            transform.rotation = Quaternion.LookRotation(ray.direction);

            ProjectileVisualData data = new()
            {
                startPosition = visualSpawnPosition,
                endPosition = Vector3.zero
            };
            visual.Play(data);

            _travelRoutine = StartCoroutine(Travel(ray, launchSpeed, targetAllegiance));
        }
    }

    protected virtual IEnumerator Travel(Ray ray, float launchSpeed, CharacterAllegiance targetAllegiance)
    {
        Vector3 projectilePosition = ray.origin;
        float distanceAlongRay = 0f;
        launchSpeed *= Time.fixedDeltaTime;

        while (true)
        {
            RaycastHit hit;
            if
            (
                _width <= 0 ?
                Physics.Raycast(projectilePosition, ray.direction, out hit, launchSpeed, targetAllegiance.GetLayerMask() | Constants.LayerMask.Environment) :
                Physics.SphereCast(projectilePosition, _width, ray.direction, out hit, launchSpeed, targetAllegiance.GetLayerMask() | Constants.LayerMask.Environment)
            )
            {
                if (hit.transform.gameObject.layer == targetAllegiance.GetLayer())
                    hit.transform.GetComponent<Hurtbox>().Owner.ReceiveDamage(impactDamage);
                Perish();
            }

            distanceAlongRay += launchSpeed;
            projectilePosition = ray.GetPoint(distanceAlongRay);
            transform.position = projectilePosition;

            yield return Constants.WaitFor.fixedUpdate;
        }
    }

    void AttemptPerish()
    {
        if (_isInstant)
            Perish();
    }

    protected override void Perish()
    {
        base.Perish();
        if (_travelRoutine != null)
            StopCoroutine(_travelRoutine);
    }
}
