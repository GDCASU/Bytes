using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Vertical Slice: https://docs.google.com/document/d/1CcRi0IGXlqaR201UI9EBUWjuiFyrSbZiKEtRt3kqeTI/edit#

/// <summary>
/// A type of drone that will seek the player, then charge at the player, and then
/// blow itself up to deal massive damage to the player.
/// 
/// Author: Alben Trang and Ryan C.
/// </summary>
public class BomberDrone : DroneBase
{
    [Header("Bomber Drone Variables")]
    [Tooltip("The amount of damage to hurt the target player")]
    [SerializeField] private int damage = 80;
    [Tooltip("The amount of force the explosion pushes objects")]
    [SerializeField] private float explosionForce = 7;
    [Tooltip("The radius of the explosion")]
    [SerializeField] private float explosionRadius = 3;
    [Tooltip("The adjustment for lifting objects from the explosion")]
    [SerializeField] private float upwardsModifier = 1;
    [Tooltip("The force mode that's applied to the rigidbody")]
    [SerializeField] private ForceMode forceMode = ForceMode.Force;
    [Tooltip("The distance it travels after its charge attack before it returns to roaming state")]
    [SerializeField] private float maxLaunchDistance = 15;
    [Tooltip("After launching, set the distance buffer for stopping its launch")]
    [SerializeField] private float launchEndbuffer = 1;
    [Tooltip("The distance it roams before reversing its direction")]
    [SerializeField] private float maxDistance = 15;
    [Tooltip("The time the drone will launch before returning to its roaming state")]
    [SerializeField] private float launchTime = 5;

    private Vector3 targetLastPosition;
    private Vector3 launchEndPosition;
    private bool readyToExplode;
    private bool isLaunching;

    protected override void Start()
    {
        base.Start();
        targetLastPosition = Vector3.zero;
        launchEndPosition  = Vector3.zero;
        readyToExplode = false;
        isLaunching = false;
    }

    protected override void FixedUpdate()
    {
        if (!isActing && Physics.Raycast(firePoint.position, firePoint.TransformDirection(Vector3.forward), out RaycastHit hit, distanceToObstacle))
        {
            ReverseDirection();
            originalPosition = this.transform.position;
        }
        else if (isLaunching)
        {
            LaunchToTarget();
        }
        else if (!isActing)
        {
            base.FixedUpdate();
        }
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
    }

    protected override void RoamingAction()
    {
        if (Vector3.Distance(transform.position, originalPosition) >= maxDistance)
        {
            ReverseDirection();
            originalPosition = this.transform.position;
        }
        else
        {
            transform.Translate(0.0f, 0.0f, droneSpeed * Time.deltaTime);
        }
    }

    protected override void InitialAction()
    {
        transform.LookAt(target.transform);

        // Chase player
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, droneSpeed * Time.deltaTime);

        // Obstacle detection starts here
        //transform.Translate(0.1f, 0.0f, 0.0f);
    }

    protected override void SecondaryAction()
    {
        isActing = true;
        originalPosition = transform.position;
        targetLastPosition = target.transform.position;
        launchEndPosition = (targetLastPosition - originalPosition).normalized * maxLaunchDistance;
        transform.LookAt(target.transform);

        StartCoroutine(ChargeBuildup());
    }

    /// <summary>
    /// Wait after a few seconds to charge before launching itself towards the target
    /// </summary>
    private IEnumerator ChargeBuildup()
    {
        yield return new WaitForSeconds(chargeTime);
        readyToExplode = true;
        isLaunching = true;
    }

    /// <summary>
    /// Move towards the direction of the target's first position when it got close to the drone and triggered it
    /// </summary>
    private void LaunchToTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, launchEndPosition, droneSpeed * Time.deltaTime);
        Collider[] colliders = Physics.OverlapBox(transform.position, transform.localScale / 2, transform.rotation, LayerMask.GetMask("Protagonist", "Default"));
        if (colliders.Length > 0)
        {
            foreach (Collider collider in colliders)
            {
                if (collider.gameObject.GetComponent<Damageable>() != null)
                {
                    collider.gameObject.GetComponent<Damageable>().ReceiveDamage(damage);
                }

                if (collider.gameObject.GetComponent<Rigidbody>() != null)
                {
                    collider.gameObject.GetComponent<Rigidbody>().AddExplosionForce(explosionForce,
                        transform.position, explosionRadius, upwardsModifier, forceMode);
                }
            }

            Destroy(gameObject);
        }
        else if (Vector3.Distance(transform.position, launchEndPosition) < launchEndbuffer)
        {
            readyToExplode = false;
            ReturnToOriginalHeight();
            isActing = false;
            isLaunching = false;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (readyToExplode)
        {
            if (collision.gameObject.GetComponent<Damageable>() != null)
            {
                collision.gameObject.GetComponent<Damageable>().ReceiveDamage(damage);
            }

            if (collision.gameObject.GetComponent<Rigidbody>() != null)
            {
                collision.gameObject.GetComponent<Rigidbody>().AddExplosionForce(explosionForce,
                    transform.position, explosionRadius, upwardsModifier, forceMode);
            }

            Destroy(gameObject);
        }
    }
}
