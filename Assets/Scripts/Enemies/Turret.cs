/*
 * Author: Cristion Dominguez
 * Date: 10 Aug. 2022
 */

using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Damageable), typeof(OpponentDetection))]
public class Turret : MonoBehaviour
{
    [Header("Rotation")]
    [SerializeField] float _horizontalRotationSpeed;
    [SerializeField] float _verticalRotationSpeed;
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
    float _horizontalAnglesPerFixedUpdate;
    float _verticalAnglesPerFixedUpdate;
    SimplePool<Projectile> _bulletPool;
    int _bulletSpawnIndex;
    WaitForSeconds _shootWait;
    OpponentDetection _detection;
    Rigidbody _opponentRB;

    void Awake()
    {
        _damageable = GetComponent<Damageable>();
        _damageable.Died += Perish;
        _detection = GetComponent<OpponentDetection>();
    }

    void OnValidate()
    {
        _horizontalAnglesPerFixedUpdate = _horizontalRotationSpeed * Time.fixedDeltaTime;
        _verticalAnglesPerFixedUpdate = _verticalRotationSpeed * Time.fixedDeltaTime;
        _shootWait = new WaitForSeconds(1f / _fireRate);
    }

    void OnEnable()
    {
        float bulletLifespan = _bullet.Lifespan;
        int capacity = bulletLifespan > 0f ? Mathf.CeilToInt(bulletLifespan * _fireRate) : _fireRate;
        _bulletPool = new SimplePool<Projectile>(_bullet, capacity + 1);
    }

    public void StartShooting()
    {
        if (_detection.OpponentDamageable.TryGetComponent(out _opponentRB))
            StartCoroutine(CR_Shoot());
    }

    public void StopShooting()
    {
        StopAllCoroutines();
    }

    IEnumerator CR_Shoot()
    {
        StartCoroutine(CR_TrackTarget());
        while (_detection.OpponentDamageable)
        {
            Transform bulletSpawn = _bulletSpawns[_bulletSpawnIndex];
            Vector3 direction = Spread.DeviateFromForwardDirection(bulletSpawn, _spread);

            Projectile projectile = _bulletPool.Get();
            projectile.transform.position = bulletSpawn.position;
            projectile.Launch(new Ray(bulletSpawn.position, direction), _launchSpeed, bulletSpawn.position, _damageable);
            _bulletSpawnIndex = _bulletSpawnIndex + 1 < _bulletSpawns.Length ? _bulletSpawnIndex + 1 : 0;

            yield return _shootWait;
        }
    }

    IEnumerator CR_TrackTarget()
    {
        while (_detection.OpponentDamageable)
        {
            RotateTowardsPosition(_detection.VisiblePointOnOpponent);
            yield return Constants.WaitFor.fixedUpdate;
        }
    }

    void RotateTowardsPosition(Vector3 point)
    {
        Vector3 predictedTargetPosition = AimPrediction.FirstOrderIntercept(_aimPivot.position, Vector3.zero, _launchSpeed, point, _opponentRB.velocity);
        predictedTargetPosition = _aimPivot.InverseTransformPoint(predictedTargetPosition);
        
        Vector2 horizontalDirection = new Vector2(predictedTargetPosition.z, -predictedTargetPosition.x).normalized;
        Vector3 verticalDirection = predictedTargetPosition.normalized;

        float yDegrees = Rotation.RotateTowardHorizontalVector(_horizontalRotator.localEulerAngles.y, horizontalDirection, _horizontalAnglesPerFixedUpdate);
        _horizontalRotator.localRotation = Quaternion.Euler(0f, yDegrees, 0f);

        float xDegrees = Rotation.RotateTowardsVerticalVector(_verticalRotator.localEulerAngles.x, verticalDirection, _verticalAnglesPerFixedUpdate);
        Quaternion verticalRotation = Quaternion.Euler(xDegrees, 0f, 0f);
        if (verticalRotation.eulerAngles.x < 360f - _detection.MaxVerticalAngle && verticalRotation.eulerAngles.x > 180f)
        {
            verticalRotation = Quaternion.Euler(360f - _detection.MaxVerticalAngle, 0f, 0f);
        }
        else if (verticalRotation.eulerAngles.x > _detection.MaxVerticalAngle && verticalRotation.eulerAngles.x < 180f)
        {
            verticalRotation = Quaternion.Euler(_detection.MaxVerticalAngle, 0f, 0f);
        }
        _verticalRotator.localRotation = verticalRotation;
    }    

    void Perish()
    {
        StopAllCoroutines();
        _detection.StopDetecting();
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
