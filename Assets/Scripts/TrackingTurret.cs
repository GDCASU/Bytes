using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingTurret : MonoBehaviour
{
    [SerializeField]
    private CharacterType targetType = CharacterType.Player;
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private Transform[] bulletSpawns;
    [SerializeField]
    private float bulletLaunchSpeed = 12f;
    [SerializeField]
    private float fireRate = 9f;
    [SerializeField]
    private float spread = 0.5f;
    [SerializeField]
    private float detectionDistance = 2f;

    [SerializeField]
    private Transform aimingPivot;
    [SerializeField]
    private float horizontalRotationSpeed = 200f;
    [SerializeField]
    private float verticalRotationSpeed = 100f;
    [SerializeField, Range(0f, 90f)]
    private float maxVerticalAngle = 60f;
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

    private void CheckForTargetsInRange()
    {
        Collider[] colliders = Physics.OverlapSphere(aimingPivot.position, detectionDistance, 1 << (int)targetType, QueryTriggerInteraction.Ignore);
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
        (Physics.Raycast(aimingPivot.position, collider.transform.position - aimingPivot.position, out hit, detectionDistance) && collider == hit.collider);

    private bool IsTargetVisible(Transform target, Transform[] detectionPoints, out Vector3 visiblePointPosition)
    {
        foreach (Transform point in detectionPoints)
        {
            if (Physics.Raycast(aimingPivot.position, point.position - aimingPivot.position, out hit, detectionDistance) &&
            hit.transform.CompareTag(target.transform.tag))
            {
                visiblePointPosition = point.position;
                return true;
            }
        }

        visiblePointPosition = Vector3.zero;
        return false;
    }

    private void TrackTarget(Vector3 visiblePointPosition)
    {
        float clockwiseDistance, counterClockwiseDistance;
        float angleStep;

        Vector3 predictedTargetCenter = PredictiveAiming.FirstOrderIntercept(aimingPivot.position, Vector3.zero, bulletLaunchSpeed, visiblePointPosition, visibleTargetBody.velocity);
        Vector3 direction = (predictedTargetCenter - aimingPivot.position).normalized;
        Vector3 horizontalDirection = new Vector2(direction.z, -direction.x).normalized;

        /*
        float yRotation1 = Mathf.Acos(horizontalDirection.x);
        float yRotation2 = Mathf.Asin(horizontalDirection.y);
        float yRotation = (yRotation2 < 0f ? yRotation1 : 2 * Mathf.PI - yRotation1) * Mathf.Rad2Deg;

        // H
        if (yRotation < horizontalRotator.eulerAngles.y)
        {
            clockwiseDistance = 360f - horizontalRotator.eulerAngles.y + yRotation;
            counterClockwiseDistance = horizontalRotator.eulerAngles.y - yRotation;
        }
        else
        {
            clockwiseDistance = yRotation - horizontalRotator.eulerAngles.y;
            counterClockwiseDistance = 360f - yRotation + horizontalRotator.eulerAngles.y;
        }

        if (clockwiseDistance < counterClockwiseDistance)
        {
            angleStep = Mathf.Min(clockwiseDistance, horizontalAnglesPerFixedUpdate);
            horizontalRotator.localRotation *= Quaternion.Euler(0f, angleStep, 0f);
        }
        else if (clockwiseDistance > counterClockwiseDistance)
        {
            angleStep = Mathf.Min(counterClockwiseDistance, horizontalAnglesPerFixedUpdate);
            horizontalRotator.localRotation *= Quaternion.Euler(0f, -angleStep, 0f);
        }

        if (horizontalRotator.localEulerAngles.y < 0f)
        {
            horizontalRotator.localRotation = Quaternion.Euler(0f, 360f + horizontalRotator.localEulerAngles.y, 0f);
        }
        else if (horizontalRotator.localEulerAngles.y > 360f)
        {
            horizontalRotator.localRotation = Quaternion.Euler(0f, horizontalRotator.localEulerAngles.y - 360f, 0f);
        }
        */ 

        float yDisplacement = Rotation.GetAngularDisplacement_2D(horizontalRotator.eulerAngles.y, horizontalDirection, horizontalAnglesPerFixedUpdate);
        horizontalRotator.localRotation *= Quaternion.Euler(0f, yDisplacement, 0f);
        if (horizontalRotator.localEulerAngles.y < 0f)
        {
            horizontalRotator.localRotation = Quaternion.Euler(0f, 360f + horizontalRotator.localEulerAngles.y, 0f);
        }
        else if (horizontalRotator.localEulerAngles.y > 360f)
        {
            horizontalRotator.localRotation = Quaternion.Euler(0f, horizontalRotator.localEulerAngles.y - 360f, 0f);
        }

        // V
        float xRotation = Mathf.Asin(direction.y) * Mathf.Rad2Deg;
        float verticalXAngle = verticalRotator.localEulerAngles.x;
        float convertedVerticalXAngle = (verticalXAngle >= 270f && verticalXAngle <= 360f) ? 360f - verticalXAngle : -verticalXAngle;
        if (xRotation < convertedVerticalXAngle)
        {
            angleStep = Mathf.Min(convertedVerticalXAngle - xRotation, verticalAnglesPerFixedUpdate);
            verticalRotator.localRotation *= Quaternion.Euler(angleStep, 0f, 0f);
        }
        else if (xRotation > convertedVerticalXAngle)
        {
            angleStep = Mathf.Min(xRotation - convertedVerticalXAngle, verticalAnglesPerFixedUpdate);
            verticalRotator.localRotation *= Quaternion.Euler(-angleStep, 0f, 0f);
        }

        verticalXAngle = verticalRotator.localEulerAngles.x;
        convertedVerticalXAngle = (verticalXAngle >= 270f && verticalXAngle <= 360f) ? 360f - verticalXAngle : -verticalXAngle;
        if (convertedVerticalXAngle < -maxVerticalAngle)
        {
            verticalRotator.localRotation = Quaternion.Euler(-maxVerticalAngle, 0f, 0f);
        }
        else if (convertedVerticalXAngle > maxVerticalAngle)
        {
            verticalRotator.localRotation = Quaternion.Euler(maxVerticalAngle, 0f, 0f);
        }
    }

    private void Shoot()
    {
        Transform bulletSpawn = bulletSpawns[bulletSpawnIndex];
        Vector3 direction = Spread.DeviatingDirection(bulletSpawn, spread);
        Instantiate(bullet, bulletSpawn.position, Quaternion.identity).GetComponent<Projectile>().Launch(new Ray(bulletSpawn.position, direction), bulletLaunchSpeed);
        bulletSpawnIndex = bulletSpawnIndex + 1 < bulletSpawns.Length ? bulletSpawnIndex + 1 : 0;

        StartCoroutine(UndergoCooldown());
    }

    private IEnumerator UndergoCooldown()
    {
        canShoot = false;
        yield return cooldownWait;
        canShoot = true;
    }

    private void Perish()
    {
        // ...
        enabled = false;
    }
}
