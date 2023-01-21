/*
 * Author: Cristion Dominguez
 * Date: 10 Aug. 2022
 */

using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Damageable))]
public class Turret : MonoBehaviour
{
    [Header("Target Detection")]
    [SerializeField] float _detectionDistance;
    [SerializeField] float _radarPulseRate;

    [Header("Rotation")]
    [SerializeField] float _horizontalRotationSpeed;
    [SerializeField] float _verticalRotationSpeed;
    [SerializeField, Range(0f, 90f)] float _maxVerticalAngle;
    [SerializeField] Transform _aimPivot;
    [SerializeField] Transform _horizontalRotator;
    [SerializeField] Transform _verticalRotator;

    [Header("Bullet")]
    [SerializeField] float _launchSpeed;
    [SerializeField] int _fireRate;
    [SerializeField] float _spread;
    [SerializeField] Projectile _bullet;
    [SerializeField] Transform[] _bulletSpawns;

    [Header("Death")]
    [SerializeField, Range(-90f, 90f)] float _droopAngle;
    [SerializeField] float _droopRotationSpeed;

    Damageable _damageable;
    Collider[] _detectedColliders = new Collider[10];
    Transform _visibleTarget;
    Rigidbody _targetBody;
    Damageable _targetDamageable;
    float _horizontalAnglesPerFixedUpdate;
    float _verticalAnglesPerFixedUpdate;
    SimplePool<Projectile> _bulletPool;
    int _bulletSpawnIndex;
    WaitForSeconds _pulseWait;
    WaitForSeconds _cooldownWait;
    RaycastHit _hit;

    void Awake()
    {
        _damageable = GetComponent<Damageable>();
        _damageable.Died += Perish;
    }

    void OnValidate()
    {
        _horizontalAnglesPerFixedUpdate = _horizontalRotationSpeed * Time.fixedDeltaTime;
        _verticalAnglesPerFixedUpdate = _verticalRotationSpeed * Time.fixedDeltaTime;
        _pulseWait = new WaitForSeconds(1f / _radarPulseRate);
        _cooldownWait = new WaitForSeconds(1f / _fireRate);
    }

    void OnEnable()
    {
        float bulletLifespan = _bullet.Lifespan;
        int capacity = bulletLifespan > 0f ? Mathf.CeilToInt(bulletLifespan * _fireRate) : _fireRate;
        _bulletPool = new SimplePool<Projectile>(_bullet, capacity + 1);
    }

    void Start() => StartCoroutine(EmitRadarPulse());

    IEnumerator EmitRadarPulse()
    {
        while (true)
        {
            while (!_visibleTarget)
            {
                CheckForTargetsInRange();
                yield return _pulseWait;
            }

            Coroutine trackRoutine = StartCoroutine(TrackTarget());
            StartCoroutine(Shoot());
            yield return trackRoutine;
        }
    }

    IEnumerator TrackTarget()
    {
        while (_visibleTarget)
        {
            Vector3 visiblePointPosition;
            if (IsTargetVisible(_visibleTarget, _targetDamageable.TargetPoints, out visiblePointPosition))
                RotateTowardsTarget(visiblePointPosition);
            else
            {
                _visibleTarget = null;
                _targetBody = null;
            }

            yield return Constants.WaitFor.fixedUpdate;
        }
    }

    IEnumerator Shoot()
    {
        while (_visibleTarget)
        {
            Transform bulletSpawn = _bulletSpawns[_bulletSpawnIndex];
            Vector3 direction = Spread.DeviateFromForwardDirection(bulletSpawn, _spread);

            Projectile projectile = _bulletPool.Get();
            projectile.transform.position = bulletSpawn.position;
            projectile.Launch(new Ray(bulletSpawn.position, direction), _launchSpeed, bulletSpawn.position, _damageable);
            _bulletSpawnIndex = _bulletSpawnIndex + 1 < _bulletSpawns.Length ? _bulletSpawnIndex + 1 : 0;

            yield return _cooldownWait;
        }
    }

    void CheckForTargetsInRange()
    {
        int count = Physics.OverlapSphereNonAlloc(_aimPivot.position, _detectionDistance, _detectedColliders, _damageable.OpponentAllegiance.GetLayerMask(), QueryTriggerInteraction.Ignore);
        for (int i = 0; i < count; i++)
        {
            Collider collider = _detectedColliders[i];
            if (IsColliderVisible(collider))
            {
                _visibleTarget = collider.transform;
                _targetBody = collider.GetComponent<Rigidbody>();
                _targetDamageable = collider.GetComponent<Damageable>();
                break;
            }
        }
    }

    bool IsColliderVisible(Collider collider) =>
        (Physics.Raycast(_aimPivot.position, collider.transform.position - _aimPivot.position, out _hit, _detectionDistance, _damageable.OpponentAllegiance.GetLayerMask() | Constants.LayerMask.Environment, QueryTriggerInteraction.Ignore) &&
        collider == _hit.collider &&
        InVerticalSight(_hit.point));

    bool IsTargetVisible(Transform target, Transform[] detectionPoints, out Vector3 visiblePointPosition)
    {
        foreach (Transform point in detectionPoints)
        {
            if (Physics.Raycast(_aimPivot.position, point.position - _aimPivot.position, out _hit, _detectionDistance, _damageable.OpponentAllegiance.GetLayerMask() | Constants.LayerMask.Environment, QueryTriggerInteraction.Ignore) &&
            _hit.transform.CompareTag(target.transform.tag) &&
            InVerticalSight(_hit.point))
            {
                visiblePointPosition = point.position;
                return true;
            }
        }

        visiblePointPosition = Vector3.zero;
        return false;
    }

    bool InVerticalSight(Vector3 position)
    {
        Vector3 direction = _aimPivot.InverseTransformPoint(position).normalized;
        float angle = Mathf.Asin(direction.y) * Mathf.Rad2Deg;
        return angle >= -_maxVerticalAngle && angle <= _maxVerticalAngle;
    }

    void RotateTowardsTarget(Vector3 visiblePointPosition)
    {
        Vector3 predictedTargetPosition = AimPrediction.FirstOrderIntercept(_aimPivot.position, Vector3.zero, _launchSpeed, visiblePointPosition, _targetBody.velocity);
        predictedTargetPosition = _aimPivot.InverseTransformPoint(predictedTargetPosition);
        Vector2 horizontalDirection = new Vector2(predictedTargetPosition.z, -predictedTargetPosition.x).normalized;
        Vector3 verticalDirection = predictedTargetPosition.normalized;
        
        float yDegrees = Rotation.RotateTowardHorizontalVector(_horizontalRotator.localEulerAngles.y, horizontalDirection, _horizontalAnglesPerFixedUpdate);
        _horizontalRotator.localRotation = Quaternion.Euler(0f, yDegrees, 0f);

        float xDegrees = Rotation.RotateTowardsVerticalVector(_verticalRotator.localEulerAngles.x, verticalDirection, _verticalAnglesPerFixedUpdate);
        Quaternion verticalRotation = Quaternion.Euler(xDegrees, 0f, 0f);
        if (verticalRotation.eulerAngles.x < 360f - _maxVerticalAngle && verticalRotation.eulerAngles.x > 180f)
        {
            verticalRotation = Quaternion.Euler(360f - _maxVerticalAngle, 0f, 0f);
        }
        else if (verticalRotation.eulerAngles.x > _maxVerticalAngle && verticalRotation.eulerAngles.x < 180f)
        {
            verticalRotation = Quaternion.Euler(_maxVerticalAngle, 0f, 0f);
        }
        _verticalRotator.localRotation = verticalRotation;
    }

    void Perish()
    {
        StopAllCoroutines();
        StartCoroutine(CR_Perish());
    }

    IEnumerator CR_Perish()
    {
        float initialXAngle = _verticalRotator.localEulerAngles.x;
        if (initialXAngle >= 270f && initialXAngle <= 360f) initialXAngle -= 360f;
        float finalXAngle = _droopAngle;
        float dyingDuration = (_droopAngle - initialXAngle) / _droopRotationSpeed;
        float elapsedTime = 0f;

        while (elapsedTime < dyingDuration)
        {
            _verticalRotator.localRotation = Quaternion.Euler(Mathf.Lerp(initialXAngle, finalXAngle, elapsedTime / dyingDuration), 0f, 0f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        _verticalRotator.localRotation = Quaternion.Euler(finalXAngle, 0f, 0f);

        _bulletPool.Dispose();
        enabled = false;
    }
}
