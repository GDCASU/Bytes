/*
 * Author: Cristion Dominguez
 * Date: 10 Aug. 2022
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class TurretAttackSystem : MonoBehaviour
{
    [Header("Target Detection")]
    [SerializeField]
    private CharacterType targetType = CharacterType.Player;
    [SerializeField]
    private float detectionDistance;
    [SerializeField]
    private float radarPulseRate;

    [Header("Rotation")]
    [SerializeField]
    private float horizontalRotationSpeed;
    [SerializeField]
    private float verticalRotationSpeed;
    [SerializeField, Range(0f, 90f)]
    private float maxVerticalAngle;
    [SerializeField]
    private Transform aimPivot;
    [SerializeField]
    private Transform horizontalRotator;
    [SerializeField]
    private Transform verticalRotator;

    [Header("Bullet")]
    [SerializeField]
    private float launchSpeed;
    [SerializeField]
    private int fireRate;
    [SerializeField]
    private float spread;
    [SerializeField]
    private Old_Projectile bullet;
    [SerializeField]
    private Transform[] bulletSpawns;

    private Transform visibleTarget;
    private Rigidbody visibleTargetBody;
    private Character visibleCharacter;

    private float horizontalAnglesPerFixedUpdate;
    private float verticalAnglesPerFixedUpdate;

    private ObjectPool<Old_Projectile> bulletPool;
    private int bulletSpawnIndex;
    private Action<Action<Old_Projectile>> notifyBulletToDestoySelf;

    private WaitForSeconds pulseWait;
    private WaitForSeconds cooldownWait;

    RaycastHit hit;

    private void OnValidate()
    {
        horizontalAnglesPerFixedUpdate = horizontalRotationSpeed * Time.fixedDeltaTime;
        verticalAnglesPerFixedUpdate = verticalRotationSpeed * Time.fixedDeltaTime;
        pulseWait = new WaitForSeconds(1f / radarPulseRate);
        cooldownWait = new WaitForSeconds(1f / fireRate);
    }

    public void Dev_OnEnable()
    {
        float bulletLifespan = bullet.Lifespan;
        int capacity = bulletLifespan > 0f ? Mathf.CeilToInt(bulletLifespan * fireRate) : fireRate;
        bulletPool = new ObjectPool<Old_Projectile>(
            () =>
            {
                Old_Projectile projectile = Instantiate(bullet);
                projectile.ReturnSelfTo((p) => bulletPool.Release(p));
                notifyBulletToDestoySelf += projectile.ReturnSelfTo;
                return projectile;
            },
            (p) => p.gameObject.SetActive(true),
            (p) => p.gameObject.SetActive(false),
            (p) =>
            {
                if (p)
                {
                    notifyBulletToDestoySelf -= p.ReturnSelfTo;
                    Destroy(p.gameObject);
                }
            },
            false,
            capacity,
            capacity + 1
        );
    }

    public void Dev_Start() => StartCoroutine(EmitRadarPulse());

    public void Dev_OnDisable()
    {
        StopAllCoroutines();
        bulletPool.Clear();
        notifyBulletToDestoySelf?.Invoke(null);
    }

    private IEnumerator EmitRadarPulse()
    {
        while (true)
        {
            while (!visibleTarget)
            {
                CheckForTargetsInRange();
                yield return pulseWait;
            }

            Coroutine trackRoutine = StartCoroutine(TrackTarget());
            StartCoroutine(Shoot());
            yield return trackRoutine;
        }
    }

    private IEnumerator TrackTarget()
    {
        while (visibleTarget)
        {
            Vector3 visiblePointPosition;
            if (IsTargetVisible(visibleTarget, visibleCharacter.DetectionPoints, out visiblePointPosition))
                RotateTowardsTarget(visiblePointPosition);
            else
            {
                visibleTarget = null;
                visibleTargetBody = null;
            }

            yield return Constants.WaitFor.fixedUpdate;
        }
    }

    private IEnumerator Shoot()
    {
        while (visibleTarget)
        {
            Transform bulletSpawn = bulletSpawns[bulletSpawnIndex];
            Vector3 direction = Spread.DeviateFromForwardDirection(bulletSpawn, spread);

            Old_Projectile projectile = bulletPool.Get();
            projectile.transform.position = bulletSpawn.position;
            projectile.Launch(new Ray(bulletSpawn.position, direction), launchSpeed, targetType, bulletSpawn.position);
            bulletSpawnIndex = bulletSpawnIndex + 1 < bulletSpawns.Length ? bulletSpawnIndex + 1 : 0;

            yield return cooldownWait;
        }
    }

    private void CheckForTargetsInRange()
    {
        Collider[] colliders = Physics.OverlapSphere(aimPivot.position, detectionDistance, targetType.GetLayerMask());
        foreach (Collider collider in colliders)
        {
            if (IsColliderVisible(collider))
            {
                visibleTarget = collider.transform.root;
                visibleTargetBody = collider.GetComponent<Rigidbody>();
                visibleCharacter = collider.GetComponent<Character>();
                break;
            }
        }
    }

    private bool IsColliderVisible(Collider collider) =>
        (Physics.Raycast(aimPivot.position, collider.transform.position - aimPivot.position, out hit, detectionDistance, targetType.GetLayerMask() | Constants.LayerMask.Environment) &&
        collider == hit.collider &&
        InVerticalSight(hit.point));

    private bool IsTargetVisible(Transform target, Transform[] detectionPoints, out Vector3 visiblePointPosition)
    {
        foreach (Transform point in detectionPoints)
        {
            if (Physics.Raycast(aimPivot.position, point.position - aimPivot.position, out hit, detectionDistance, targetType.GetLayerMask() | Constants.LayerMask.Environment) &&
            hit.transform.CompareTag(target.transform.tag) &&
            InVerticalSight(hit.point))
            {
                visiblePointPosition = point.position;
                return true;
            }
        }

        visiblePointPosition = Vector3.zero;
        return false;
    }

    private bool InVerticalSight(Vector3 position)
    {
        Vector3 direction = aimPivot.InverseTransformPoint(position).normalized;
        float angle = Mathf.Asin(direction.y) * Mathf.Rad2Deg;
        return angle >= -maxVerticalAngle && angle <= maxVerticalAngle;
    }

    private void RotateTowardsTarget(Vector3 visiblePointPosition)
    {
        Vector3 predictedTargetPosition = AimPrediction.FirstOrderIntercept(aimPivot.position, Vector3.zero, launchSpeed, visiblePointPosition, visibleTargetBody.velocity);
        predictedTargetPosition = aimPivot.InverseTransformPoint(predictedTargetPosition);
        Vector2 horizontalDirection = new Vector2(predictedTargetPosition.z, -predictedTargetPosition.x).normalized;
        Vector3 verticalDirection = predictedTargetPosition.normalized;
        
        float yDegrees = Rotation.RotateTowardHorizontalVector(horizontalRotator.localEulerAngles.y, horizontalDirection, horizontalAnglesPerFixedUpdate);
        horizontalRotator.localRotation = Quaternion.Euler(0f, yDegrees, 0f);

        float xDegrees = Rotation.RotateTowardsVerticalVector(verticalRotator.localEulerAngles.x, verticalDirection, verticalAnglesPerFixedUpdate);
        Quaternion verticalRotation = Quaternion.Euler(xDegrees, 0f, 0f);
        if (verticalRotation.eulerAngles.x < 360f - maxVerticalAngle && verticalRotation.eulerAngles.x > 180f)
        {
            verticalRotation = Quaternion.Euler(360f - maxVerticalAngle, 0f, 0f);
        }
        else if (verticalRotation.eulerAngles.x > maxVerticalAngle && verticalRotation.eulerAngles.x < 180f)
        {
            verticalRotation = Quaternion.Euler(maxVerticalAngle, 0f, 0f);
        }
        verticalRotator.localRotation = verticalRotation;
    }
}
