using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAttackSystem : MonoBehaviour
{
    [Header("Target Detection")]
    [SerializeField]
    private CharacterType targetType = CharacterType.Player;
    [SerializeField]
    private float detectionDistance = 2f;

    [Header("Bullet")]
    [SerializeField]
    private float launchSpeed = 12f;
    [SerializeField]
    private float fireRate = 9f;
    [SerializeField]
    private float spread = 0.5f;
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private Transform[] bulletSpawns;

    [Header("Rotation")]
    [SerializeField]
    private float horizontalRotationSpeed = 200f;
    [SerializeField]
    private float verticalRotationSpeed = 100f;
    [SerializeField, Range(0f, 90f)]
    private float maxVerticalAngle = 60f;
    [SerializeField]
    private Transform aimPivot;
    [SerializeField]
    private Transform horizontalRotator;
    [SerializeField]
    private Transform verticalRotator;

    private int bulletSpawnIndex = 0;
    private float horizontalAnglesPerFixedUpdate;
    private float verticalAnglesPerFixedUpdate;

    private Transform visibleTarget;
    private Rigidbody visibleTargetBody;
    private ICharacter visibleCharacter;

    private bool canShoot = true;
    private WaitForSeconds cooldownWait;
    private Coroutine cooldownRoutine;

    RaycastHit hit;

    private void Awake()
    {
        horizontalAnglesPerFixedUpdate = horizontalRotationSpeed * Time.fixedDeltaTime;
        verticalAnglesPerFixedUpdate = verticalRotationSpeed * Time.fixedDeltaTime;
        cooldownWait = new WaitForSeconds(1f / fireRate);
    }

    private void Update()
    {
        if (visibleTarget && canShoot)
        {
            Shoot();
        }
    }

    private void FixedUpdate()
    {
        if (!visibleTarget)
        {
            CheckForTargetsInRange();
        }
        else
        {
            Vector3 visiblePointPosition;
            if (IsTargetVisible(visibleTarget, visibleCharacter.GetDetectionPoints(), out visiblePointPosition))
                TrackTarget(visiblePointPosition);
            else
            {
                visibleTarget = null;
                visibleTargetBody = null;
            }
        }
    }

    private void OnDisable()
    {
        if (cooldownRoutine != null)
        {
            StopCoroutine(cooldownRoutine);
        }
    }

    private void CheckForTargetsInRange()
    {
        Collider[] colliders = Physics.OverlapSphere(aimPivot.position, detectionDistance, 1 << (int)targetType);
        foreach (Collider collider in colliders)
        {
            if (IsColliderVisible(collider))
            {
                visibleTarget = collider.transform;
                visibleTargetBody = collider.GetComponent<Rigidbody>();
                visibleCharacter = collider.GetComponent<ICharacter>();
                break;
            }
        }
    }

    private bool IsColliderVisible(Collider collider) =>
        (Physics.Raycast(aimPivot.position, collider.transform.position - aimPivot.position, out hit, detectionDistance) &&
        collider == hit.collider &&
        InVerticalSight(hit.point));

    private bool IsTargetVisible(Transform target, Transform[] detectionPoints, out Vector3 visiblePointPosition)
    {
        foreach (Transform point in detectionPoints)
        {
            if (Physics.Raycast(aimPivot.position, point.position - aimPivot.position, out hit, detectionDistance) &&
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

    private void TrackTarget(Vector3 visiblePointPosition)
    {
        Vector3 predictedTargetPosition = PredictiveAiming.FirstOrderIntercept(aimPivot.position, Vector3.zero, launchSpeed, visiblePointPosition, visibleTargetBody.velocity);
        predictedTargetPosition = aimPivot.InverseTransformPoint(predictedTargetPosition);
        Vector3 direction = predictedTargetPosition.normalized;
        Vector2 horizontalDirection = new Vector2(direction.z, -direction.x).normalized;

        float yDisplacement = Rotation.GetAngularDisplacement_2D(horizontalRotator.localEulerAngles.y, horizontalDirection, horizontalAnglesPerFixedUpdate);
        horizontalRotator.localRotation *= Quaternion.Euler(0f, yDisplacement, 0f);

        float xDisplacement = Rotation.GetVerticalAngularDisplacement(verticalRotator.localEulerAngles.x, direction, verticalAnglesPerFixedUpdate);
        Quaternion verticalRotation = verticalRotator.localRotation * Quaternion.Euler(xDisplacement, 0f, 0f);
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

    private void Shoot()
    {
        Transform bulletSpawn = bulletSpawns[bulletSpawnIndex];
        Vector3 direction = Spread.DeviateFromForwardDirection(bulletSpawn, spread);
        Instantiate(bullet, bulletSpawn.position, Quaternion.identity).GetComponent<Projectile>().Launch(new Ray(bulletSpawn.position, direction), launchSpeed);
        bulletSpawnIndex = bulletSpawnIndex + 1 < bulletSpawns.Length ? bulletSpawnIndex + 1 : 0;

        cooldownRoutine = StartCoroutine(UndergoCooldown());
    }

    private IEnumerator UndergoCooldown()
    {
        canShoot = false;
        yield return cooldownWait;
        canShoot = true;
        cooldownRoutine = null;
    }
}
